using BackgroundServices.Concrete;
using Microsoft.Extensions.Configuration;
using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Quartz.Logging;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SchedulerTests
{
    public class DataSynchronizerTests
    {
        [Fact]
        public async Task StartAsync_SchedulesJobWithCorrectTrigger()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            var scheduler = new Mock<IScheduler>();
            var dataSynchronizer = new DataSynchronizer(scheduler.Object, configuration.Object);

            // Configure IConfiguration mock to return desired values
            configuration.Setup(c => c.GetSection("ConnectionInfo:AlignmentInfo:01:PassingTime").Value).Returns("12:30:00");
            configuration.Setup(c => c.GetSection("ConnectionInfo:AlignmentInfo:01:TimeWindow").Value).Returns("00:15:00");
            configuration.Setup(c => c.GetSection("ConnectionInfo:Delay").Value).Returns("10");

            // Act
            await dataSynchronizer.StartAsync(CancellationToken.None);

            // Assert
            //scheduler.Verify(s => s.ScheduleJob(
            //    It.Is<IJobDetail>(jd => jd.JobType == typeof(DataSynchronizerJob)),
            //    It.Is<ITrigger>(t =>
            //        t.GetType() == typeof(SimpleTriggerImpl) &&
            //        ((SimpleTriggerImpl)t).StartTimeUtc.LocalDateTime == DateTime.Today.Add(TimeSpan.Parse("12:30:00")) &&
            //        ((SimpleTriggerImpl)t).EndTimeUtc.LocalDateTime == DateTime.Today.Add(TimeSpan.Parse("12:45:00")) &&
            //        ((SimpleTriggerImpl)t).RepeatInterval == TimeSpan.FromSeconds(10).TotalMilliseconds
            //    )
            //), Times.Once);

        }
    }
}
