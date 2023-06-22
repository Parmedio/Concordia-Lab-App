namespace ReportSender.MailSenderLogic.Abstract;

public interface IMailSender
{
    internal (bool, string) SendEmail(string reportId, string attachmentPath);
}
