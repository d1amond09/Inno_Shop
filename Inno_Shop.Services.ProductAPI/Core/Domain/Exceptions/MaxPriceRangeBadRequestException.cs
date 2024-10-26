namespace Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;

public class MaxPriceRangeBadRequestException : BadRequestException
{
	public MaxPriceRangeBadRequestException() : base("Max price can't be less than min price.")
	{

	}
}
