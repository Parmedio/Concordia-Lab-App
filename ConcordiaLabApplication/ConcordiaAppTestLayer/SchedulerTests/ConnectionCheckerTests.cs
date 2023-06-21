using Moq;
using Scheduler;
using System.Net;

namespace ConcordiaAppTestLayer.SchedulerTests;

public class ConnectionCheckerTests
{
    private readonly HttpClient _httpClient;
    private readonly ConnectionChecker _sut;

    public ConnectionCheckerTests()
    {
        _httpClient = Mock.Of<HttpClient>();
        _sut = new ConnectionChecker(_httpClient);
    }
     
    [Fact]
    public async void Should_Return_True_With_Connection()
    {
        IHttpClientWrapper httpClient = new HttpClientWrapper();
        HttpResponseMessage response = await httpClient.GetAsync("https://api.trello.com");

        var httpClientMock = new Mock<IHttpClientWrapper>();
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        httpClientMock
            .Setup(x => x.GetAsync("https://api.trello.com"))
            .ReturnsAsync(expectedResponse);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
        
    [Fact]
    public async void Should_Return_False_With_No_Connection()
    {
        IHttpClientWrapper httpClient = new HttpClientWrapper();
        HttpResponseMessage response = await httpClient.GetAsync("https://api.trello.com");

        var httpClientMock = new Mock<IHttpClientWrapper>();
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
        httpClientMock
            .Setup(x => x.GetAsync("https://api.trello.com"))
            .ReturnsAsync(expectedResponse);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}