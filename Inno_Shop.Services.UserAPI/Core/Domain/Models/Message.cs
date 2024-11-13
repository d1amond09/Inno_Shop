using MimeKit;
using MailKit.Net.Smtp;

namespace Inno_Shop.Services.UserAPI.Core.Domain.Models;

public class Message(IEnumerable<string> to, string subject, string content, IFormFileCollection? attachments)
{
    public List<MailboxAddress> To { get; set; } = 
        [.. to.Select(x => new MailboxAddress("", x))];
    public string Subject { get; set; } = subject;
    public string Content { get; set; } = content;
    public IFormFileCollection? Attachments { get; set; } = attachments;
}