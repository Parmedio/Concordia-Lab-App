using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Jobs;

public class MonthlyTriggerJob : IJob
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MonthlyTriggerJob> _logger;

    public MonthlyTriggerJob(ISchedulerFactory schedulerFactory, IConfiguration configuration, ILogger<MonthlyTriggerJob> logger)
    {
        _schedulerFactory = schedulerFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var dataSyncJobKey = new JobKey("DataSynchronizerJob");
            var dataSyncJobDetail = await scheduler.GetJobDetail(dataSyncJobKey);
            var dataSyncJobTriggers = await scheduler.GetTriggersOfJob(dataSyncJobKey);

            if (dataSyncJobTriggers.Any())
            {
                var dataSyncJobTrigger = dataSyncJobTriggers.First();
                var newTrigger = CreateNewTrigger();
                await scheduler.RescheduleJob(dataSyncJobTrigger.Key, newTrigger); 
            }
            else
            {
                var dataSyncJob = JobBuilder.Create<DataSynchronizerJob>()
                        .WithIdentity("DataSynchronizerJob")
                        .Build();
                var newTrigger = CreateNewTrigger();
                await scheduler.ScheduleJob(dataSyncJob, newTrigger);
            }
        }
        catch (SchedulerException ex)
        {
            _logger.LogError(ex, "Error occurred in MonthlyTriggerJob");
        }
    }


    private ITrigger CreateNewTrigger()
    {
        var connectionInfo = _configuration.GetSection("ConnectionInfo");
        var alignmentInfo = connectionInfo.GetSection("AlignmentInfo");
        foreach (var section in alignmentInfo.GetChildren())
        {
            var month = section.Key;
            var jobKey = section.Key;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var targetMonth = int.Parse(month);

            if (currentMonth == targetMonth)
            {
                var passingTime = section["PassingTime"];
                var timeWindow = section["TimeWindow"];
                var delay = int.Parse(connectionInfo.GetSection("Delay").Value!);

                var passingTimeSpan = TimeSpan.Parse(passingTime!);
                var timeWindowSpan = TimeSpan.Parse(timeWindow!);
                var sum = passingTimeSpan + timeWindowSpan;

                var startTime = DateTimeOffset.Now.Date.Add(passingTimeSpan);
                var endTime = DateTimeOffset.Now.Date.Add(sum);

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("DynamicTrigger")
                    .StartAt(startTime)
                    .EndAt(endTime)
                    .WithSimpleSchedule(builder =>
                        builder.WithIntervalInSeconds(delay)
                            .RepeatForever())
                    .Build();

                _logger.LogInformation($"The synchronization to the {currentMonth} month of the year {currentYear} starts at {startTime.Hour}:{startTime.Minute} and ends at {endTime.Hour}:{endTime.Minute}.");

                return trigger;
            }
        }
        throw new Exception();
    }
}
