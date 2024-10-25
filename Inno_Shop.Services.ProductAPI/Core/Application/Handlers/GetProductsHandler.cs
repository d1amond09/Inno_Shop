using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

public class GetProductsHandler(IProductRepository rep, IMapper mapper) : IRequestHandler<GetProductsQuery, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;

	public async Task<ApiBaseResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		var products = await _rep.GetProductsAsync(request.TrackChanges);
		var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
		return new ApiOkResponse<IEnumerable<ProductDto>>(productsDto);
	}
}
