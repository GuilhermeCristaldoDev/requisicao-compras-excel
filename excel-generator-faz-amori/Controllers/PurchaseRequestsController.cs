using ExcelGenerator.Api.DTOs;
using ExcelGenerator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExcelGenerator.Api.Controllers;

[ApiController]
[Route("api/purchase-requests")]
public class PurchaseRequestsController : ControllerBase
{
    private readonly ExcelGeneratorService _excelService;
    private readonly PdfGeneratorService _pdfService;
    private readonly IWebHostEnvironment _env;

    public PurchaseRequestsController(
        ExcelGeneratorService excelService,
        PdfGeneratorService pdfService,
        IWebHostEnvironment env)
    {
        _excelService = excelService;
        _pdfService = pdfService;
        _env = env;
    }

    [HttpPost("excel")]
    public IActionResult GenerateExcel([FromBody] CreatePurchaseRequestDto dto)
    {
        var bytes = _excelService.Generate(dto);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "requisicao-compras.xlsx");
    }

    [HttpPost("pdf")]
    public IActionResult GeneratePdf([FromBody] CreatePurchaseRequestDto dto)
    {
        var bytes = _excelService.Generate(dto);

        var tempDir = Path.Combine(_env.ContentRootPath, "Temp");
        Directory.CreateDirectory(tempDir);

        var excelPath = Path.Combine(tempDir, $"requisicao-{Guid.NewGuid()}.xlsx");
        var pdfPath = Path.ChangeExtension(excelPath, ".pdf");

        System.IO.File.WriteAllBytes(excelPath, bytes);

        var generatedPdfPath = _pdfService.ConvertExcelToPdf(excelPath);

        var pdfBytes = System.IO.File.ReadAllBytes(generatedPdfPath);

        // limpeza
        System.IO.File.Delete(excelPath);
        System.IO.File.Delete(generatedPdfPath);

        return File(pdfBytes, "application/pdf", "requisicao-compras.pdf");
    }
}
