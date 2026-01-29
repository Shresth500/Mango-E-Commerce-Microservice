using Mango.Services.EmailAPI.Messaging;

namespace Mango.Services.RewardAPI.Extensions;

public static class ApplicationBuilderExtension
{
    private static IAzureServiceBusConsumer azureServiceBusConsumer; 
    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        azureServiceBusConsumer = app.ApplicationServices.GetRequiredService<IAzureServiceBusConsumer>();
        var hostApplicationLife = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        hostApplicationLife.ApplicationStarted.Register(OnStart);
        hostApplicationLife.ApplicationStopping.Register(OnStop);

        return app;
    }

    private static void OnStop()
    {
        azureServiceBusConsumer.Stop();
    }

    private static void OnStart()
    {
        azureServiceBusConsumer.Start();
    }
}
