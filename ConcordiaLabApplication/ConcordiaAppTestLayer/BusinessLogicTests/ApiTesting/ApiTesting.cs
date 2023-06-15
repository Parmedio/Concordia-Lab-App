using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;

using FluentAssertions;

using Moq;

namespace ConcordiaAppTestLayer.BusinessLogicTests.ApiTesting;

public class ApiTesting
{

    private readonly IApiReceiver _receiver;
    private readonly IApiSender _sender;
    private readonly IUriCreatorFactory uriFactoryMock;
    private readonly IHttpClientFactory _factoryMock;
    private readonly Uri baseUrl;
    private static int commentCount = 1;


    public ApiTesting()
    {
        baseUrl = new Uri("https://api.trello.com/1/");
        _factoryMock = Mock.Of<IHttpClientFactory>();
        uriFactoryMock = Mock.Of<IUriCreatorFactory>();
        _receiver = new ApiReceiver(_factoryMock, uriFactoryMock);
        _sender = new ApiSender(_factoryMock, uriFactoryMock);

    }

    [Fact]
    public async Task GetAllExperimentsInToDoList_ShouldReturn1Experiment()
    {

        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCardsOnToDoList()).Returns("lists/6482fa302b3165154f5b6e99/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        var experiments = await _receiver.GetAllExperimentsInToDoList();

        experiments.Should().HaveCount(1);
        experiments!.FirstOrDefault()!.Name.Should().Be("EsperimentoDiProva");
        experiments!.FirstOrDefault()!.TrelloColumnId.Should().Be("6482fa302b3165154f5b6e99");
        experiments!.FirstOrDefault()!.Id.Should().Be("6482fba0e13f2eaf24ec081f");
        experiments!.FirstOrDefault()!.Desc.Should().Be("ShortDesc");
        experiments!.FirstOrDefault()!.Due.Should().Be(new DateTime(2023, 6, 10, 13, 0, 0, DateTimeKind.Utc));
        experiments!.FirstOrDefault()!.IdMembers.Should().Equal(new List<string>() { "639c692ed850f6055714fd55" });
        experiments!.FirstOrDefault()!.IdLabels.Should().Equal(new List<string>() { "6482fa27eaa9d8e931fe2f2a" });
    }

    [Fact]
    public async Task GetAllExperimentsInToDoList_ShouldThrowException()
    {

        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCardsOnToDoList()).Returns("lists/aa/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        var experiments = _receiver.GetAllExperimentsInToDoList();
        await _receiver.Invoking(p => p.GetAllExperimentsInToDoList()).Should().ThrowAsync<HttpRequestException>();

    }

    [Fact]
    public async Task GetAllComments_ShouldReturn1Comment()
    {
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCommentsOnABoard()).Returns("boards/6482fa27402ed2e69e93493f/actions?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD&filter=commentCard");
        var comments = await _receiver.GetAllComments();
        comments.Should().HaveCount(1);
        commentCount = comments.Count();
        var commentToAnalyze = comments!.Where(p => p.Id == "64833e3ba2cde375a521486a").SingleOrDefault();
        commentToAnalyze.Should().NotBeNull();
        commentToAnalyze.IdMemberCreator.Should().Be("5bf9f901921c336b20b29d25");
        commentToAnalyze.Data.Text.Should().Be("CommentoSemplice");
        commentToAnalyze.Data.Card.Id.Should().Be("6482fba0e13f2eaf24ec081f");
        commentToAnalyze.MemberCreator.Username.Should().Be("alessandroferlugaubaldini");


    }

    [Fact]
    public async Task GetAllComments_ShouldThrowHttpRequestException()
    {
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCommentsOnABoard()).Returns("boards/aaa/actions?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD&filter=commentCard");
        await _receiver.Invoking(p => p.GetAllComments()).Should().ThrowAsync<Exception>();

    }

    [Fact]
    public async Task MoveExperiment_ShouldMoveTheExperiment()
    {
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        var cardID = "6482fba0e13f2eaf24ec081f";
        var listId = "6482fa39b72a7053b7b07e17";
        var listId2 = "6482fa302b3165154f5b6e99";
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.UpdateAnExperiment(cardID, listId)).Returns("cards/6482fba0e13f2eaf24ec081f?idList=6482fa39b72a7053b7b07e17&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        await _sender.Invoking(p => p.UpdateAnExperiment(cardID, listId)).Should().NotThrowAsync();


        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCardsOnToDoList()).Returns("lists/6482fa302b3165154f5b6e99/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        var experiments = await _receiver.GetAllExperimentsInToDoList();
        experiments.Should().HaveCount(0);

        Mock.Get(uriFactoryMock).Setup(p => p.UpdateAnExperiment(cardID, listId2)).Returns("cards/6482fba0e13f2eaf24ec081f?idList=6482fa302b3165154f5b6e99&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        await _sender.UpdateAnExperiment(cardID, listId2);

        experiments = await _receiver.GetAllExperimentsInToDoList();
        experiments.Should().HaveCount(1);

    }

    [Fact]
    public async Task AddComment_ShouldIncreaseTheNumberOfCommentsBy1()
    {
        using var client = new HttpClient()
        {
            BaseAddress = baseUrl
        };
        var cardID = "6482fba0e13f2eaf24ec081f";
        var textToAdd = "Ciao Sono un commento";
        var token = "ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
        Mock.Get(_factoryMock).Setup(_ => _.CreateClient("ApiConsumer")).Returns(client);
        Mock.Get(uriFactoryMock).Setup(p => p.AddACommentOnACard(cardID, textToAdd, token)).Returns("cards/6482fba0e13f2eaf24ec081f/actions/comments?text=Ciao Sono un commento&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
        Mock.Get(uriFactoryMock).Setup(p => p.GetAllCommentsOnABoard()).Returns("boards/6482fa27402ed2e69e93493f/actions?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD&filter=commentCard");

        await _sender.Invoking(p => p.AddAComment(cardID, textToAdd, token)).Should().NotThrowAsync();
        var result = await _receiver.GetAllComments();
        result.Should().HaveCount(2);

        var newId = result.Where(p => p.Id != "64833e3ba2cde375a521486a").Select(p => p.Id).FirstOrDefault();
        await client.DeleteAsync($"actions/{newId}?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");

        result = await _receiver.GetAllComments();
        result.Should().HaveCount(1);

    }
}
