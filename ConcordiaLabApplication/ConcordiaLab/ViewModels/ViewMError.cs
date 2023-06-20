namespace WebApp.ViewModels;

public class ViewMError
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}