using BusinessLogic.DataTransferLogic.Abstract;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using Quartz;

using Scheduler;
using Scheduler.Jobs;

namespace ConcordiaAppTestLayer.SchedulerTests;
public class DataSynchronizerJobTests
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSynchronizerJob> _logger;
    private readonly IClientService _clientService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConnectionChecker _connectionChecker;
    private readonly DataSynchronizerJob _sut;

    public DataSynchronizerJobTests()
    {
        _configuration = Mock.Of<IConfiguration>();
        _logger = Mock.Of<ILogger<DataSynchronizerJob>>();
        _clientService = Mock.Of<IClientService>();
        _serviceScopeFactory = Mock.Of<IServiceScopeFactory>();
        _connectionChecker = Mock.Of<IConnectionChecker>();
        _sut = new DataSynchronizerJob(_logger, _serviceScopeFactory);
    }

    [Fact]
    public async Task Execute_Should_Update_Connection_State_When_Connection_Achieved()
    {
        Mock.Get(_connectionChecker).Setup(x => x.CheckConnection()).ReturnsAsync(true);
        Mock.Get(_clientService).Setup(x => x.UpdateConnectionStateAsync(true)).ReturnsAsync(true);
        Mock.Get(_clientService).Setup(x => x.SyncDataAsyncs()).Returns(Task.CompletedTask);

        var jobContextMock = new Mock<IJobExecutionContext>();

        await _sut.Execute(jobContextMock.Object);


        // Assert
        //Mock.Get(_connectionChecker).Verify(x => x.CheckConnection(), Times.Once);
        //Mock.Get(_clientService).Verify(x => x.UpdateConnectionStateAsync(true), Times.Once);
        //Mock.Get(_clientService).Verify(x => x.SyncDataAsyncs(), Times.Once);
        Mock.Get(_logger).Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
    ),
    Times.Once
);
    }

    [Fact]
    public async Task Execute_Should_Update_Connection_State_When_Connection_Is_Not_Available()
    {
        Mock.Get(_connectionChecker).Setup(x => x.CheckConnection()).ReturnsAsync(false);
        Mock.Get(_clientService).Setup(x => x.UpdateConnectionStateAsync(false)).ReturnsAsync(false);
        Mock.Get(_clientService).Setup(x => x.SyncDataAsyncs()).Returns(Task.CompletedTask);

        var jobContextMock = new Mock<IJobExecutionContext>();

        // Act
        await _sut.Execute(jobContextMock.Object);

        // Assert
        //Mock.Get(_connectionChecker).Verify(x => x.CheckConnection(), Times.Once);
        //Mock.Get(_clientService).Verify(x => x.UpdateConnectionStateAsync(true), Times.Once);
        //Mock.Get(_clientService).Verify(x => x.SyncDataAsyncs(), Times.Once);
    }
}