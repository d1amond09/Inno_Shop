using Inno_Shop.Services.UserAPI.Core.Domain.Models;

namespace Inno_Shop.Services.UserAPI.Core.Application.Contracts;

public interface IEmailSender
{
    void SendEmail(Message message);
    Task SendEmailAsync(Message message);
}