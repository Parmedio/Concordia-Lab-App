using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using Scheduler.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConcordiaAppTestLayer.SchedulerTests
{
    public class MonthlyTriggerJobTests
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MonthlyTriggerJob> _logger;
        private readonly MonthlyTriggerJob _sut;

        public MonthlyTriggerJobTests()
        {
            _schedulerFactory = Mock.Of<ISchedulerFactory>();
            _configuration = Mock.Of<IConfiguration>();
            _logger = Mock.Of<ILogger<MonthlyTriggerJob>>();
            _sut = new MonthlyTriggerJob(_schedulerFactory, _configuration, _logger);
        }

        [Fact]
        public async Task Execute_Should_Reschedule_Job_When_Trigger_Exists()
        {
            var schedulerMock = new Mock<IScheduler>();
            var jobDetailMock = new Mock<IJobDetail>();
            var triggerMock = new Mock<ITrigger>();

            //schedulerMock.Setup(x => x.GetJobDetail(It.IsAny<JobKey>())).ReturnsAsync(jobDetailMock.Object);
            //schedulerMock.Setup(x => x.GetTriggersOfJob(It.IsAny<JobKey>())).ReturnsAsync(new[] { triggerMock.Object });
            //schedulerMock.Setup(x => x.RescheduleJob(It.IsAny<TriggerKey>(), It.IsAny<ITrigger>())).Returns(Task.CompletedTask);
            //_schedulerFactory.Setup(x => x.GetScheduler()).ReturnsAsync(schedulerMock.Object);
             
            // Act
            await _sut.Execute(null);

            // Assert
            //schedulerMock.Verify(x => x.GetJobDetail(It.IsAny<JobKey>()), Times.Once);
            //schedulerMock.Verify(x => x.GetTriggersOfJob(It.IsAny<JobKey>()), Times.Once);
            //schedulerMock.Verify(x => x.RescheduleJob(It.IsAny<TriggerKey>(), It.IsAny<ITrigger>()), Times.Once);
            //schedulerMock.Verify(x => x.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()), Times.Never);
        }

        [Fact]
        public async Task Execute_Should_Schedule_New_Job_When_Trigger_Does_Not_Exist()
        {
            var schedulerMock = new Mock<IScheduler>();
            var jobDetailMock = new Mock<IJobDetail>();

            //schedulerMock.Setup(x => x.GetJobDetail(It.IsAny<JobKey>())).ReturnsAsync(jobDetailMock.Object);
            //schedulerMock.Setup(x => x.GetTriggersOfJob(It.IsAny<JobKey>())).ReturnsAsync(Enumerable.Empty<ITrigger>());
            //schedulerMock.Setup(x => x.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>())).Returns(Task.CompletedTask);
            //_schedulerFactory.Setup(x => x.GetScheduler()).ReturnsAsync(schedulerMock.Object);

            // Act
            await _sut.Execute(null);

            // Assert 
            //schedulerMock.Verify(x => x.GetJobDetail(It.IsAny<JobKey>()), Times.Once);
            //schedulerMock.Verify(x => x.GetTriggersOfJob(It.IsAny<JobKey>()), Times.Once);
            //schedulerMock.Verify(x => x.RescheduleJob(It.IsAny<TriggerKey>(), It.IsAny<ITrigger>()), Times.Never);
            //schedulerMock.Verify(x => x.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()), Times.Once);
        }
    }
}
