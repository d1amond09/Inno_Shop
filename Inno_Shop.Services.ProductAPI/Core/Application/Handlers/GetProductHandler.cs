using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using Inno_Shop.Services.ProductAPI.Domain.Models;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

public class GetProductHandler(IProductRepository rep, IMapper mapper, IDataShaper<ProductDto> dataShaper) : IRequestHandler<GetProductQuery, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;
	private readonly IDataShaper<ProductDto> _dataShaper = dataShaper;

	public async Task<ApiBaseResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
	{
		var product = await _rep.GetProductByIdAsync(request.ProductId, request.TrackChanges);
		if (product is null)
			return new ProductNotFoundResponse(request.ProductId);

		var productDto = _mapper.Map<ProductDto>(product);
        return new ApiOkResponse<ProductDto>(productDto);
	}
}
