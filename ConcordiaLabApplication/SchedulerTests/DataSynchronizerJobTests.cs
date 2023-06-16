using BackgroundServices.Abstract;
using BackgroundServices.Concrete;
using BusinessLogic.DataTransferLogic.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SchedulerTests;
public class DataSynchronizerJobTests
{
    [Fact]
    public async Task Execute_ConnectionAvailable_CallsDataServiceAndLogger()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        var scopeFactory = new Mock<IServiceScopeFactory>();
        var logger = new Mock<ILogger<DataSynchronizerJob>>();
        var clientService = new Mock<IClientService>();
        var connectionChecker = new Mock<IConnectionChecker>();

        // Configure the mocks as needed
        configuration.Setup(c => c.GetSection("ConnectionInfo:...").Value).Returns("...");
        scopeFactory.Setup(sf => sf.CreateScope()).Returns(Mock.Of<IServiceScope>());
        clientService.Setup(ds => ds.UpdateConnectionStateAsync(It.IsAny<bool>())).ReturnsAsync(true);
        connectionChecker.Setup(cc => cc.CheckConnection()).ReturnsAsync(true);

        // Create an instance of DataSynchronizerJob
        var dataSynchronizerJob = new DataSynchronizerJob(configuration.Object, scopeFactory.Object, logger.Object);

        // Act
        await dataSynchronizerJob.Execute(Mock.Of<IJobExecutionContext>());

        // Assert
        logger.Verify(l => l.LogInformation("Connection Established!"), Times.Once);
        clientService.Verify(ds => ds.UpdateConnectionStateAsync(true), Times.Once);
        clientService.Verify(ds => ds.SyncDataAsyncs(), Times.Once);
        logger.Verify(l => l.LogInformation("Executed new cycle. current connection state is Online"), Times.Once);
    }
}
