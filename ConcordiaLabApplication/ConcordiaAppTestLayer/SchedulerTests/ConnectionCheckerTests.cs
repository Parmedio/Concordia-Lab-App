using Moq;
using Scheduler;

namespace ConcordiaAppTestLayer.SchedulerTests;

public class ConnectionCheckerTests
{
    private readonly ConnectionChecker _sut;

    public ConnectionCheckerTests()
        => _sut = new ConnectionChecker(new HttpClient());

    [Fact]
    public async void Should_Return_True_With_Connection()
        => Assert.True(await _sut.CheckConnection());

    [Fact]
    public async void Should_Return_Fasle_With_No_Connection()
    {
        var connectionCheckerMock = new Mock<IConnectionChecker>();
        connectionCheckerMock.Setup(x => x.CheckConnection()).ReturnsAsync(false); 
        Assert.False(await connectionCheckerMock.Object.CheckConnection());
    }
}