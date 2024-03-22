using Database.Extensions;
using NLog.Extensions.Logging;
using Server.AI;
using Server.Data;
using Server.Data.Interfaces;
using Server.Services;
using Server.Services.Interfaces;
using TicTacToeAI.AI;
using TicTacToeAI.AI.Interfaces;

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
        builder.Services.AddScoped<IAiManager, AiManager>();
        builder.Services.AddScoped<IBot, SimpleBot>();
        
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
        app.MapControllers();

        app.Run();
    }
}