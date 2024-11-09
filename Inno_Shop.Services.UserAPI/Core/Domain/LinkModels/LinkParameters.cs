using Inno_Shop.Services.UserAPI.Core.Domain.RequestFeatures;

namespace Inno_Shop.Services.UserAPI.Core.Domain.LinkModels;

public record LinkParameters(UserParameters UserParameters, HttpContext Context);
