using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl.Matchers;
using Quartz;
using Scheduler.Jobs;
using Microsoft.Extensions.Logging;

namespace ConcordiaAppTestLayer.SchedulerTests;
    public class JobsTriggersConfigurationTests
    {
        private readonly IScheduler _scheduler;

        public JobsTriggersConfigurationTests()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole());
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJob<MonthlyTriggerJob>(opts => opts.WithIdentity("MonthlyTriggerJob"))
                    .AddTrigger(opts => opts
                        .ForJob("MonthlyTriggerJob")
                        .StartNow()
                        .WithSimpleSchedule(builder =>
                            builder.WithInterval(TimeSpan.FromDays(28))
                                .RepeatForever()));

                q.AddJob<DataSynchronizerJob>(opts => opts.WithIdentity("DataSynchronizerJob"))
                    .AddTrigger(opts => opts
                    .WithIdentity("DynamicTrigger")
                    .ForJob("DataSynchronizerJob")
                    .StartAt(DateTimeOffset.MaxValue));

                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            });

            var serviceProvider = services.BuildServiceProvider();
            var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerFactory>();
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
            _scheduler = scheduler;
        }

        [Fact]
        public void Quartz_Service_Configuration_Is_Valid()
        {
            Assert.NotNull(_scheduler);

            Assert.Equal(2, _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result.Count);

            var dataSynchronizerJobKey = new JobKey("DataSynchronizerJob");
            var dataSynchronizerJobDetail = _scheduler.GetJobDetail(dataSynchronizerJobKey).GetAwaiter().GetResult();
            Assert.NotNull(dataSynchronizerJobDetail);
            Assert.Equal(typeof(DataSynchronizerJob), dataSynchronizerJobDetail.JobType);

            var monthlyTriggerJobKey = new JobKey("DataSynchronizerJob");
            var monthlyTriggerJobjobDetail = _scheduler.GetJobDetail(monthlyTriggerJobKey).GetAwaiter().GetResult();
            Assert.NotNull(monthlyTriggerJobjobDetail);

            Assert.Equal(3, _scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).Result.Count);

            var triggerDynamicKey = new TriggerKey("DynamicTrigger");
            var dynamicTrigger = _scheduler.GetTrigger(triggerDynamicKey).GetAwaiter().GetResult();
            Assert.NotNull(dynamicTrigger);
            Assert.Equal("DataSynchronizerJob", dynamicTrigger.JobKey.Name);
        }

        [Fact]
        public void MonthlyTriggerJob_Should_Start_Now()
        {
            var monthlyTriggerJobKey = new JobKey("MonthlyTriggerJob");
            var monthlyTriggerJobTriggers = _scheduler.GetTriggersOfJob(monthlyTriggerJobKey).GetAwaiter().GetResult();
            var monthlyTrigger = monthlyTriggerJobTriggers.First();

            var expectedStartTime = new DateTimeOffset(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month, DateTimeOffset.UtcNow.Day, DateTimeOffset.UtcNow.Hour, DateTimeOffset.UtcNow.Minute, DateTimeOffset.UtcNow.Second, TimeSpan.Zero);
            var actualStartTime = new DateTimeOffset(monthlyTrigger.StartTimeUtc.Year, monthlyTrigger.StartTimeUtc.Month, monthlyTrigger.StartTimeUtc.Day, monthlyTrigger.StartTimeUtc.Hour, monthlyTrigger.StartTimeUtc.Minute, monthlyTrigger.StartTimeUtc.Second, TimeSpan.Zero);
            Assert.Equal(expectedStartTime, actualStartTime);
        }
    }