

using BusinessLogic.APIConsumers.Abstract;
using BusinessLogic.APIConsumers.Concrete;
using BusinessLogic.APIConsumers.UriCreators;

namespace ConcordiaLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<IApiReceiver, ApiReceiver>();
            builder.Services.AddTransient<IUriCreatorFactory, UriCreatorFactory>();
            builder.Services.AddTransient<IApiSender, ApiSender>();
            builder.Services.AddTransient<Test1>();
            builder.Services.AddLogging();

            var app = builder.Build();

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


            app.Services.GetService<Test1>().Run();
            app.Run();
        }
    }
}