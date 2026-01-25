using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;


namespace Mango.MessageBus;

public class MessageBus : IMessageBus
{
    public async Task PublishMessage(object message, string topic_queue_Name)
    {
        var connectionString = ConnectionStrings.ServiceBusConnectionString;
        await using var client = new ServiceBusClient(connectionString);
        var sender = client.CreateSender(topic_queue_Name);
        var jsonMessage = JsonConvert.SerializeObject(message);
        var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString()
        };
        await sender.SendMessageAsync(finalMessage);
        await client.DisposeAsync();
    }
}
