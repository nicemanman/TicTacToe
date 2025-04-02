using MessageQueue.Hosted;
using MessageQueue.Options;
using MessageQueue.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageQueue.Extensions;

public static class RabbitExtension
{
	public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<RabbitOptions>(configuration.GetSection(RabbitOptions.SectionName));
		services.AddSingleton<BackgroundJob>();
		services.AddHostedService(x => x.GetRequiredService<BackgroundJob>());
		services.AddSingleton<IMqSender>(x => x.GetRequiredService<BackgroundJob>());
		services.AddScoped<IMqReceiver>(x => x.GetRequiredService<BackgroundJob>());
		
		return services;
	}
	
	
}