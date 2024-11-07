using System.ComponentModel.Design;
using System.Dynamic;
using AutoMapper;
using Inno_Shop.Services.ProductAPI.Core.Application.Contracts;
using Inno_Shop.Services.ProductAPI.Core.Application.Queries;
using Inno_Shop.Services.ProductAPI.Core.Application.Service;
using Inno_Shop.Services.ProductAPI.Core.Domain.Exceptions;
using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Core.Domain.Models;
using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;
using Inno_Shop.Services.ProductAPI.Core.Domain.Responses;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;
using MediatR;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Handlers;

public class GetProductsHandler(IProductRepository rep, IMapper mapper, IDataShaper<ProductDto> dataShaper, IProductLinks productLinks) : 
    IRequestHandler<GetProductsQuery, ApiBaseResponse>
{
	private readonly IProductRepository _rep = rep;
	private readonly IMapper _mapper = mapper;
    private readonly IDataShaper<ProductDto> _dataShaper = dataShaper;
	private readonly IProductLinks _productLinks = productLinks;

    public async Task<ApiBaseResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
        if(request.LinkParameters.ProductParameters.NotValidPriceRange)
            return new ApiMaxPriceRangeBadRequestResponse();

        var productsWithMetaData = await _rep.GetProductsAsync(request.LinkParameters.ProductParameters, request.TrackChanges);
		var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsWithMetaData);

        var links = _productLinks.TryGenerateLinks(productsDto, request.LinkParameters.ProductParameters.Fields, request.LinkParameters.Context);
		(LinkResponse, MetaData) result = new(links, productsWithMetaData.MetaData);
        
        return new ApiOkResponse<(LinkResponse, MetaData)>(result);
	}
}
