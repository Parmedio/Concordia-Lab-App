using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.DTOs.TrelloDtos;

using System.Text.Json;

namespace ConcordiaLab;

public class Test1
{
    private IApiReceiver _receiver;

    public Test1(IApiReceiver receiver)
    {
        _receiver = receiver;
    }

    public void Run()
    {
        var a = JsonSerializer.Serialize<IEnumerable<TrelloExperimentDto>>(_receiver.GetAllExperimentsInToDoList().Result);
        Console.WriteLine(a);
    }
}
