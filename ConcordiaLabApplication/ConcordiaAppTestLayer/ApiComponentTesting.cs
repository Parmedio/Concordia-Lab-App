using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.DTOs.TrelloDtos;

using Moq;

using System.Text.Json;

namespace ConcordiaAppTestLayer
{
    public class ApiComponentTesting
    {

        private readonly IApiReceiver _receiver;

        public ApiComponentTesting()
        {
            var dependency3 = Mock.Of<IUriCreatorFactory>();
            Mock.Get(dependency3).Setup(propa => propa.GetAllCardsOnToDoList()).Returns(new Uri("https://api.trello.com/1/lists/64760975fbea80d6ef329080/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD"));
            _receiver = new ApiReceiver(new HttpClient(), dependency3);
        }

        [Fact]
        public void Test1()
        {
            List<TrelloExperimentDto> trelloExperimentList = new List<TrelloExperimentDto>()
            {
                new TrelloExperimentDto()
                {
                    Id = "6478b6501cbe9a6039368c98",
                    Name = "Scheda Di Prova",
                    IdMembers = new List<string>()
                    {
                        "5bf9f901921c336b20b29d25"
                    },
                    Due = JsonSerializer.Deserialize<DateTime>(JsonSerializer.Serialize("2023-06-04T10:00:00.000Z")),
                    IdLabels = new List<string>()
                    {
                        "647609751afdaf2b05536cd7"
                    },
                    Desc = "Che bella giornata"
                }
            };

            var ApiResultList = _receiver.GetAllExperimentsInToDoList().Result!.ToList()!;
            foreach (var element in a)


        }
    }
}