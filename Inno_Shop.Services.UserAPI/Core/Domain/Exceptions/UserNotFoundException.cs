namespace Inno_Shop.Services.UserAPI.Core.Domain.Exceptions;

public class UserNotFoundException(Guid id) : 
	NotFoundException($"The user with id: {id} doesn't exist in the database.")
{
}
