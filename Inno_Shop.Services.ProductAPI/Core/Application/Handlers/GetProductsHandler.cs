using System.ComponentModel.Design;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
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
		var productsWithMetaData = await _rep.GetProductsAsync(request.ProductParameters, request.TrackChanges);
		var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsWithMetaData);
		(IEnumerable<ProductDto> products, MetaData metaData) result = new(productsDto, productsWithMetaData.MetaData);
		return new ApiOkResponse<(IEnumerable<ProductDto>, MetaData)>(result);
	}
}
