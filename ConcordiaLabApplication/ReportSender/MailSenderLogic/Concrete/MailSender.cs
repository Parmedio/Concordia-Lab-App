using MailKit.Net.Smtp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using MimeKit;

using ReportSender.Mail_Models;
using ReportSender.MailSenderLogic.Abstract;

namespace ReportSender.MailSenderLogic.Concrete;

public class MailSender : IMailSender
{
    private readonly MailInformation _information;
    private readonly List<MailReceivers> _receivers;

    public MailSender(IOptions<MailInformation> information, IOptionsSnapshot<List<MailReceivers>> receivers, IConfiguration configuration)
    {
        _information = information.Value;
        _receivers = receivers.Value;
    }
    (bool, string) IMailSender.SendEmail(int reportNumber, string attachmentPath)
    {
        try
        {
            using MimeMessage emailMessage = new MimeMessage();
            MailboxAddress emailFrom = new MailboxAddress(_information.SenderName, _information.SenderEmail);
            emailMessage.From.Add(emailFrom);
            emailMessage.To.AddRange(_receivers.Select(p => new MailboxAddress(p.Name, p.Email)));
            emailMessage.Subject = $"Report n. {reportNumber.ToString()}";
            BodyBuilder emailBodyBuilder = new BodyBuilder();
            //FileStream stream = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read);
            emailBodyBuilder.Attachments.Add(attachmentPath);
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(_information.Server, _information.Port, false);
            mailClient.Authenticate(_information.UserName, _information.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return (true, "Mail Sent Successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Mail was not sent, error: {ex.Message}{Environment.NewLine}{ex.InnerException?.Message ?? ""}");
        }
    }
}
