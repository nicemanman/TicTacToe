using Database.Extensions;
using NLog.Extensions.Logging;
using Server.Data;
using Server.Data.Interfaces;
using Server.Services;
using Server.Services.Interfaces;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

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
        var app = builder.Build();

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