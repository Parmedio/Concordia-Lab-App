using BackgroundServices;

using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;
using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using ConcordiaLab.Controllers;
using ConcordiaLab.Controllers.Mock_Data;
using ConcordiaLab.Controllers.Mock_Data.Mock_Gateway;
using Microsoft.EntityFrameworkCore;
using PersistentLayer.Configurations;
using PersistentLayer.Repositories.Abstract;
using PersistentLayer.Repositories.Concrete;

namespace ConcordiaLab
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient("ApiConsumer", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetSection("TrelloUrlToUse")!.GetSection("baseUrl").Value!);
                client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(builder.Configuration.GetSection("ClientInfo").GetSection("timeout").Value!));
            });
            builder.Services.AddScoped<IApiSender, ApiSender>();
            builder.Services.AddTransient<IUriCreatorFactory, UriCreatorFactory>();
            builder.Services.AddLogging();
            builder.Services.AddSingleton<ConnectionChecker>();
            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddTransient<IRetrieveConnectionTimeInterval, RetrieveConnectionTimeInterval>();
            builder.Services.AddHostedService(provider => provider.GetRequiredService<ConnectionChecker>());
            builder.Services.AddTransient<IDataHandlerFactory, DataHandlerFactory>();
            builder.Services.AddTransient<ClientService>();

            //builder.Services.AddDbContext<ConcordiaDbContext>(options =>
            //      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddScoped<IExperimentRepository, ExperimentRepository>();

            builder.Services.AddScoped<MockGatewayScientist>();
            builder.Services.AddScoped<MockGatewayExperiment>();
            builder.Services.AddScoped<MockGatewayList>();
            builder.Services.AddSingleton<UserSetting>();

            var app = builder.Build();


            //await app.MigrateAsync();

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
}