using System.Text;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class SyncResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    private StringBuilder _messageBuilder = new StringBuilder();
    public string Message
    {
        get => _messageBuilder.ToString();
    }

    public int ItemCount => Items.Count();

    public SyncResult()
    {

    }

    public SyncResult(IEnumerable<T> items, StringBuilder stringBuilder)
    {
        Items = items;
        _messageBuilder = stringBuilder;
    }

    public SyncResult(IEnumerable<T> items, string Message)
    {
        Items = items;
        _messageBuilder.Append(Message);
    }

    public void Append(string information)
    {
        _messageBuilder.Append(information);
    }

    public void AppendLine(string information)
    {
        _messageBuilder.AppendLine(information);
    }
}
