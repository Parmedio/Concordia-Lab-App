namespace ReportSender.MailSenderLogic.Abstract;

public interface IMailSender
{
    internal (bool, string) SendEmail(int reportNumber, string attachmentPath);
}
