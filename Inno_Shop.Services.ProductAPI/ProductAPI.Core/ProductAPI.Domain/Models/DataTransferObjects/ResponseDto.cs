namespace Inno_Shop.Services.ProductAPI.Models.DataTransferObjects;

public class ResponseDto
{
    public bool Success { get; set; } = true;
    public object? Result { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> ErrorMessages { get; set; } = [];
}
