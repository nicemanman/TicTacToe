using Database.Extensions;
using NLog.Extensions.Logging;
using Server.Data;
using Server.Data.Interfaces;
using Server.Services;
using Server.Services.Interfaces;
using ArtificialIntelligence.AI;
using ArtificialIntelligence.AI.Interfaces;
using MessageQueue.Extensions;
using Server.Middlewares;
using Server.OpponentManager;
using Server.SignalR;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog();
        });
        builder.Services.AddUnitOfWork<IUnitOfWork, UnitOfWork>(builder.Configuration);
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IBotManager, AiManager>();
        builder.Services.AddScoped<IBot, SimpleBot>();
        builder.Services.AddScoped<IGameSessionManager, GameSessionManager>();
        builder.Services.AddScoped<IJoinCodeService, JoinCodeService>();
        builder.Services.AddSingleton<ImplicitRegistrationMiddleware>();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddRabbitMq(builder.Configuration);
        builder.Services.AddSignalR();
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            db.Migrate();
        }
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseTransaction();
        app.UseSession();
        app.UseMiddleware<ImplicitRegistrationMiddleware>();
        app.MapControllers();
        app.MapHub<GameHub>("/gameHub");
        app.Run();
    }
}