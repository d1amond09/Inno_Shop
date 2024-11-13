namespace Inno_Shop.Services.ProductAPI.Core.Domain.Responses;

public sealed class ApiMaxPriceRangeBadRequestResponse() : 
	ApiBadRequestResponse($"Max price can't be less than min price")
{
}

