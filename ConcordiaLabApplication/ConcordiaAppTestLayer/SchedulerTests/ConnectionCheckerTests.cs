using Moq;
using Moq.Protected;
using Scheduler;
using System.Net;

namespace ConcordiaAppTestLayer.SchedulerTests;

public class ConnectionCheckerTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly IConnectionChecker _sut;

    public ConnectionCheckerTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _sut = new ConnectionChecker(_httpClient);
    }

    [Fact]
    public async Task Should_Return_True_With_Connection()
    {
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var connection = await _sut.CheckConnection();

        Assert.True(connection);
    }

    [Fact]
    public async void Should_Return_False_With_No_Connection()
    {
        _httpMessageHandlerMock
    .Protected()
    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var connection = await _sut.CheckConnection();

        Assert.False(connection);
    }
}