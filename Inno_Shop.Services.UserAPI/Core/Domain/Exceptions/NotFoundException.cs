namespace Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;

public abstract class NotFoundException(string message) : Exception(message)
{
}
