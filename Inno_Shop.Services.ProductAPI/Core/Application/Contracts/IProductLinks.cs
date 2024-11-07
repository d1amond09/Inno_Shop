using Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;
using Inno_Shop.Services.ProductAPI.Domain.DataTransferObjects;

namespace Inno_Shop.Services.ProductAPI.Core.Application.Contracts;

public interface IProductLinks
{
    LinkResponse TryGenerateLinks(
        IEnumerable<ProductDto> employeesDto, 
        string fields, 
        HttpContext httpContext);
  
}
