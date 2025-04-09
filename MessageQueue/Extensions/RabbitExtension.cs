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
		services.AddSingleton<RabbitClient>();
		services.AddHostedService(x => x.GetRequiredService<RabbitClient>());
		services.AddSingleton<IMqClient>(x => x.GetRequiredService<RabbitClient>());
		
		return services;
	}
	
	
}