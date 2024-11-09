using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Inno_Shop.Services.UserAPI.Presentation.CustomTokenProviders;

public class EmailConfirmationTokenProvider<TUser>(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<EmailConfirmationTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<TUser>> logger) : 
    DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger) 
    where TUser : class
{
}
public class EmailConfirmationTokenProviderOptions : 
    DataProtectionTokenProviderOptions
{
}
