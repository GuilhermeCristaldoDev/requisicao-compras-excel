namespace ExcelGenerator.Api.DTOs;

public class CreatePurchaseRequestDto
{
    public string NumeroRequisicao { get; set; } = default!;
    public string Solicitante { get; set; } = default!;
    public string Area { get; set; } = default!;
    public DateTime Data { get; set; }
    public bool DentroDoOrcado { get; set; }

    public List<CreatePurchaseItemDto> Itens { get; set; } = new();

    public DateTime PrazoRecebimento { get; set; }

    public string? Fornecedor1 { get; set; }
    public string? Fornecedor2 { get; set; }
    public string? Fornecedor3 { get; set; }
}
