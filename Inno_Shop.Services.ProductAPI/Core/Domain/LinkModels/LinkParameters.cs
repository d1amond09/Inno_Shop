using Inno_Shop.Services.ProductAPI.Core.Domain.RequestFeatures;

namespace Inno_Shop.Services.ProductAPI.Core.Domain.LinkModels;

public record LinkParameters(ProductParameters ProductParameters, HttpContext Context);
