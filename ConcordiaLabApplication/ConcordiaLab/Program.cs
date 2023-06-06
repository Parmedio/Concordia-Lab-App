
using AutoMapper;

using BackgroundServices;

using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.AutomapperProfiles;
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
        builder.Services.AddScoped<IDataService, DataService>();
        builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();
        builder.Services.AddTransient<IUriCreatorFactory, UriCreatorFactory>();
        builder.Services.AddTransient<IDataSyncer, DataSyncer>();
        builder.Services.AddTransient<IRetrieveConnectionTimeInterval, RetrieveConnectionTimeInterval>();
        builder.Services.AddTransient<IDataHandlerFactory, DataHandlerFactory>();
        builder.Services.AddTransient<ClientService>();


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