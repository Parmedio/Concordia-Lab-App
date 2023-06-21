using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.AutomapperProfiles;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using ConcordiaLab.AutomapperViewProfile;
using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Quartz;
using Scheduler;
using Scheduler.Jobs;

namespace ConcordiaLab;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        MapperConfigurationExpression configuration = new MapperConfigurationExpression();
        configuration.AddProfiles(new List<Profile>() { new MainProfile(), new ViewProfile() });
        configuration.AddCollectionMappers();
        var mappingConfiguration = new MapperConfiguration(configuration);
        mappingConfiguration.AssertConfigurationIsValid();

        builder.Services.AddControllersWithViews();

        builder.Services.AddHttpClient("ApiConsumer", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("TrelloUrlToUse")!.GetSection("baseUrl").Value!);
            client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("ClientInfo").GetSection("timeout").Value!));
        })
            .AddPolicyHandler(GetRetryPolicy());

        builder.Services.AddDbContext<ConcordiaDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MainProfile), typeof(ViewProfile));
            cfg.AllowNullDestinationValues = true;
        });


        builder.Services.AddLogging();

        builder.Services.AddScoped<IApiSender, ApiSender>();
        builder.Services.AddScoped<IApiReceiver, ApiReceiver>();
        builder.Services.AddScoped<DataService>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddScoped<IExperimentDownloader, ExperimentDownloader>();
        builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
        builder.Services.AddScoped<IScientistRepository, ScientistRepository>();

        builder.Services.AddTransient<IDataSyncer, DataSyncer>();
        builder.Services.AddTransient<IUriCreatorFactory, UriCreatorFactory>();
        builder.Services.AddTransient<IDataHandlerFactory, DataHandlerFactory>();
        builder.Services.AddTransient<IClientService, ClientService>();
        builder.Services.AddTransient<ICommentDownloader, CommentDownloader>();
        builder.Services.AddTransient<IExperimentDownloader, ExperimentDownloader>();
        builder.Services.AddTransient<IUploader, Uploader>();

        builder.Services.AddSingleton<IConnectionChecker, ConnectionChecker>();
        builder.Services.AddScoped<DataSynchronizerJob>();
        builder.Services.AddScoped<MonthlyTriggerJob>();

        builder.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.AddJob<MonthlyTriggerJob>(opts => opts.WithIdentity("MonthlyTriggerJob"))
                .AddTrigger(opts => opts
                    .ForJob("MonthlyTriggerJob")
                    .StartNow()
                    .WithSimpleSchedule(builder =>
                        builder.WithIntervalInSeconds(5)
                            .RepeatForever()));

            q.AddJob<DataSynchronizerJob>(opts => opts.WithIdentity("DataSynchronizerJob"))
                .AddTrigger(opts => opts
                .WithIdentity("DynamicTrigger")
                .ForJob("DataSynchronizerJob")
                .StartAt(DateTimeOffset.MaxValue));

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        });

        var app = builder.Build();
        await app.MigrateAsync();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
       app.Run();
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(3), retryCount: 3);
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(delay);
    }
}