namespace ExcelGenerator.Api.DTOs;

public class CreatePurchaseItemDto
{
    public string Tipo { get; set; } = default!;
    public int Quantidade { get; set; }
    public string Descricao { get; set; } = default!;
    public decimal Valor { get; set; } // 🔥 valor unitário
}

