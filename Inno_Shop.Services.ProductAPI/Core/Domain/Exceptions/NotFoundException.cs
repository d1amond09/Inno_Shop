namespace Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;

public abstract class NotFoundException(string message) : Exception(message)
{
}
