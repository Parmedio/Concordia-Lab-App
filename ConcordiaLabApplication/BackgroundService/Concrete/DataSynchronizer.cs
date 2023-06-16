using BackgroundServices.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// funge da punto di ingresso per configurare il job e il trigger nello scheduler. Quando il metodo StartAsync viene chiamato, crea il job detail e il trigger, quindi li associa nello scheduler tramite il metodo ScheduleJob. In questo modo, quando lo scheduler viene avviato, il job verrà eseguito secondo le specifiche del trigger.
/// </summary>
public class DataSynchronizer : IHostedService
{
    private readonly IScheduler _scheduler;
    private readonly IConfiguration _configuration;

    public DataSynchronizer(IScheduler scheduler, IConfiguration configuration)
    {
        _scheduler = scheduler;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await UpdateSchedulerConfiguration();

        var currentMonth = DateTime.Now.Month.ToString("D2");
        var passingTime = _configuration.GetSection($"ConnectionInfo:AlignmentInfo:{currentMonth}:PassingTime").Value!;
        var timeWindow = TimeSpan.Parse(_configuration.GetSection($"ConnectionInfo:AlignmentInfo:{currentMonth}:TimeWindow").Value!);
        var endTime = TimeSpan.Parse(passingTime).Add(timeWindow);
        var startDate = DateTimeOffset.Now.Date.Add(TimeSpan.Parse(passingTime));
        var endDate = DateTimeOffset.Now.Date.Add(endTime);
        var delay = (int) TimeSpan.Parse(_configuration.GetSection("ConnectionInfo:Delay").Value!).TotalSeconds;

        var trigger = TriggerBuilder.Create()
            .WithIdentity("DataSynchronizerTrigger")
            .StartAt(startDate)
            .EndAt(endDate)
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(delay)
                .RepeatForever())
            .Build();

        var jobDetail = JobBuilder.Create<DataSynchronizerJob>()
            .WithIdentity("DataSynchronizerJob")
            .Build();

        await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        await _scheduler.Start(cancellationToken);
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task UpdateSchedulerConfiguration()
    {
        var currentMonth = DateTime.Now.Month.ToString("D2");
        var passingTime = _configuration.GetSection($"ConnectionInfo:AlignmentInfo:{currentMonth}:PassingTime").Value!;
        var startDate = DateTimeOffset.Now.Date.Add(TimeSpan.Parse(passingTime));
        var timeWindow = TimeSpan.Parse(_configuration.GetSection($"ConnectionInfo:AlignmentInfo:{currentMonth}:TimeWindow").Value!);
        var endTime = TimeSpan.Parse(passingTime).Add(timeWindow);
        var endDate = DateTimeOffset.Now.Date.Add(endTime);
        var delay = (int)TimeSpan.Parse(_configuration.GetSection("ConnectionInfo:Delay").Value!).TotalSeconds;

        // Ottieni il trigger corrente dallo scheduler
        var existingTrigger = await _scheduler.GetTrigger(new TriggerKey("DataSynchronizerTrigger"));

        // Crea un nuovo trigger con i nuovi valori di configurazione
        var newTrigger = TriggerBuilder.Create()
            .WithIdentity("DataSynchronizerTrigger")
            .StartAt(startDate)
            .EndAt(endDate)
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(delay)
                .RepeatForever())
            .Build();

        await _scheduler.RescheduleJob(existingTrigger.Key, newTrigger);
    }
}
