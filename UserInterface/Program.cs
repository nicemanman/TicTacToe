using BitzArt.Blazor.Cookies;
using NLog.Extensions.Logging;
using UserInterface.Components;
using UserInterface.Data;
using UserInterface.MessageHandlers;
using UserInterface.Services;

namespace UserInterface;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.Services.AddLogging(x =>
        {
            x.ClearProviders();
            x.AddNLog();
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient("MyClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("TicTacToeServer"));
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseCookies = false
            })
            .AddHttpMessageHandler<CopyCookieHandler>()
            .SetHandlerLifetime(TimeSpan.FromDays(1));
        
        builder.Services.AddTransient<CopyCookieHandler>();
        builder.Services.AddScoped<GameService>();
        builder.Services.AddScoped<UsernameData>();
        builder.Services.AddScoped<CookieData>();
        builder.AddBlazorCookies();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        app.Run();
    }
}