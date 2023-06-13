
using AutoMapper;

using BackgroundServices;
using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.AutomapperProfiles;
using ConcordiaLab.AutomapperViewProfile;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaLab;

public class Program
{
    public static async Task Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.

        MapperConfigurationExpression configuration = new MapperConfigurationExpression();
       
        configuration.AddProfile(typeof(MainProfile));
        configuration.AddProfile(typeof(ViewProfile));

        var mappingConfiguration = new MapperConfiguration(configuration);

        mappingConfiguration.AssertConfigurationIsValid();


        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpClient("ApiConsumer", client =>
        {

            client.BaseAddress = new Uri(builder.Configuration.GetSection("TrelloUrlToUse")!.GetSection("baseUrl").Value!);
            client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("ClientInfo").GetSection("timeout").Value!));
        });

        builder.Services.AddDbContext<ConcordiaDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddAutoMapper(cfg => cfg.AddProfile(configuration));

        builder.Services.AddHostedService(provider => provider.GetRequiredService<ConnectionChecker>());
        builder.Services.AddLogging();

        builder.Services.AddSingleton<ConnectionChecker>();
        builder.Services.AddScoped<IApiSender, ApiSender>();
        builder.Services.AddScoped<IApiReceiver, ApiReceiver>();
        builder.Services.AddScoped<DataService>();
        builder.Services.AddScoped<IExperimentDownloader, ExperimentDownloader>();
        builder.Services.AddTransient<IDataSyncer, DataSyncer>();

        builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IListRepository, ListRepository>();
        builder.Services.AddScoped<IScientistRepository, ScientistRepository>();

        builder.Services.AddTransient<IUriCreatorFactory, UriCreatorFactory>();
        builder.Services.AddTransient<IRetrieveConnectionTimeInterval, RetrieveConnectionTimeInterval>();
        builder.Services.AddTransient<IDataHandlerFactory, DataHandlerFactory>();
        builder.Services.AddTransient<IClientService, ClientService>();
        builder.Services.AddTransient<ICommentDownloader, CommentDownloader>();
        builder.Services.AddTransient<IExperimentDownloader, ExperimentDownloader>();
        builder.Services.AddTransient<IUploader, Uploader>();


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
}