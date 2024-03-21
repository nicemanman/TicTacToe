using Database.Interfaces;
using Database.Middlewares;
using Database.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions;

/// <summary>
/// Расширения для работы с UnitOfWork
/// </summary>
public static class UnitOfWorkExtension
{
    /// <summary>
    /// Добавить зависимости для работы с UnitOfWork
    /// </summary>
    public static void AddUnitOfWork<I, T>(this IServiceCollection services, IConfiguration configuration) 
        where I: class 
        where T: class, IBaseUnitOfWork, I
    {
        services.Configure<DbConnectionOptions>(configuration.GetSection(DbConnectionOptions.SectionName));
        services.AddScoped<DatabaseTransactionMiddleware>();
        services.AddScoped<IBaseUnitOfWork, T>();
        services.AddScoped<I, T>(x=>(T)x.GetRequiredService<IBaseUnitOfWork>());
    }

    /// <summary>
    /// Использовать транзакцию при работе с базой данных
    /// </summary>
    public static void UseTransaction(this WebApplication application)
    {
        application.UseMiddleware<DatabaseTransactionMiddleware>();
    }
}