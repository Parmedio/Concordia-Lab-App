using System.Text;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class SyncResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int _errorCount { get; private set; } = 0;
    private StringBuilder _messageBuilder = new StringBuilder();
    public string Message
    {
        get => _messageBuilder.ToString();
    }

    public int ItemCount => Items.Count();

    public SyncResult()
    {

    }

    public SyncResult(IEnumerable<T> items, StringBuilder stringBuilder, int errorCount)
    {
        Items = items;
        _messageBuilder = stringBuilder;
        _errorCount = errorCount;
    }

    public SyncResult(IEnumerable<T> items, string Message)
    {
        Items = items;
        _messageBuilder.Append(Message);
    }

    public void Append(string information, bool error = false)
    {
        _messageBuilder.Append(information);
        if (error)
            _errorCount++;
    }

    public void AppendLine(string information, bool error = false)
    {
        _messageBuilder.AppendLine(information);
        if (error)
            _errorCount++;
    }
}
