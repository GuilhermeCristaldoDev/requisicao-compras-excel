using ClosedXML.Excel;
using ExcelGenerator.Api.DTOs;
using System.Globalization;

namespace ExcelGenerator.Api.Services;

public class ExcelGeneratorService
{
    private readonly IWebHostEnvironment _env;

    public ExcelGeneratorService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public byte[] Generate(CreatePurchaseRequestDto dto)
    {
        var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "requisicao-template.xlsx");
        var logoPath = Path.Combine(_env.ContentRootPath, "Templates", "logo.png");

        using var workbook = new XLWorkbook(templatePath);
        workbook.CalculateMode = XLCalculateMode.Auto;

        var ws = workbook.Worksheet(1);

        ReplaceText(ws, "{{NUM_REQUISICAO}}", dto.NumeroRequisicao);
        ReplaceText(ws, "{{SOLICITANTE}}", dto.Solicitante);
        ReplaceText(ws, "{{AREA}}", dto.Area);
        ReplaceText(ws, "{{DATA}}", dto.Data.ToString("dd/MM/yyyy"));
        ReplaceText(ws, "{{PRAZO}}", dto.PrazoRecebimento.ToString("dd/MM/yyyy"));
        ReplaceText(ws, "{{FORN1}}", dto.Fornecedor1);
        ReplaceText(ws, "{{FORN2}}", dto.Fornecedor2);
        ReplaceText(ws, "{{FORN3}}", dto.Fornecedor3);
        ReplaceText(ws, "{{ORCADO}}", dto.DentroDoOrcado ? "Sim" : "Não");

        var (startRow, endRow) = FillItems(ws, dto.Itens);
        InsertTotal(ws, startRow, endRow);

        workbook.RecalculateAllFormulas(); // força cálculo no servidor

        InsertLogo(ws, logoPath);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private void ReplaceText(IXLWorksheet ws, string placeholder, string? value)
    {
        value ??= string.Empty;

        foreach (var cell in ws.CellsUsed())
        {
            if (cell.HasFormula)
                continue;

            if (cell.DataType == XLDataType.Text && cell.GetString().Trim() == placeholder)
                cell.Value = value;
        }
    }

    private (int startRow, int endRow) FillItems(IXLWorksheet ws, List<CreatePurchaseItemDto> items)
    {
        var markerCell = ws.CellsUsed()
            .FirstOrDefault(c => c.DataType == XLDataType.Text && c.GetString().Trim() == "{{ITENS_START}}");

        if (markerCell == null || items == null || items.Count == 0)
            return (0, 0);

        var startRow = markerCell.Address.RowNumber;

        // Remove o marcador
        markerCell.Value = "";

        // Copia a linha base ANTES de inserir os valores
        var templateRow = ws.Row(startRow);

        for (int i = 0; i < items.Count; i++)
        {
            var rowIndex = startRow + i;

            if (i > 0)
            {
                ws.Row(rowIndex - 1).InsertRowsBelow(1);
                templateRow.CopyTo(ws.Row(rowIndex));
            }

            var item = items[i];

            ws.Cell(rowIndex, "C").Value = i + 1;                 // Item
            ws.Cell(rowIndex, "D").Value = item.Tipo;             // Tipo
            ws.Cell(rowIndex, "E").Value = item.Quantidade;       // Quantidade
            ws.Cell(rowIndex, "F").Value = item.Descricao;        // Descrição

            // Valor total do item = Quantidade * Valor unitário
            ws.Cell(rowIndex, "L").FormulaA1 =
                $"E{rowIndex}*{item.Valor.ToString(CultureInfo.InvariantCulture)}";

            ws.Cell(rowIndex, "L").Style.NumberFormat.Format = "R$ #,##0.00";
        }

        var endRow = startRow + items.Count - 1;
        return (startRow, endRow);
    }

    private void InsertTotal(IXLWorksheet ws, int startRow, int endRow)
    {
        if (startRow == 0 || endRow == 0)
            return;

        var totalRow = endRow + 1;

        var totalCell = ws.Cell(totalRow, "L");
        totalCell.FormulaA1 = $"SUM(L{startRow}:L{endRow})";
        totalCell.Style.NumberFormat.Format = "R$ #,##0.00";
    }

    private void InsertLogo(IXLWorksheet ws, string logoPath)
    {
        if (!File.Exists(logoPath))
            return;

        ws.AddPicture(logoPath)
          .MoveTo(ws.Cell("C3"))
          .Scale(0.4);
    }
}
