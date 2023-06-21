namespace ReportSender.Mail_Models;

public record MailReceivers()
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}
