using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly string serviceBusConnectionString;
    private readonly string emailCartQueue;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;
    private ServiceBusProcessor _emailCartProcessor, _registeredUserProcessor, _emailOrderPlacedProcessor;
    private readonly string registeredUser;
    private readonly string orderCreatedTopic;
    private readonly string orderCreatedTopicSubscription;

    public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
    {
        _configuration = configuration;
        _emailService = emailService;

        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")!;
        emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCart")!;
        registeredUser = _configuration.GetValue<string>("TopicAndQueueNames:RegisteredUser")!;

        orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic")!;
        orderCreatedTopicSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription")!;

        var client = new ServiceBusClient(serviceBusConnectionString);
        _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        _registeredUserProcessor = client.CreateProcessor(registeredUser);
        _emailOrderPlacedProcessor = client.CreateProcessor(orderCreatedTopic,orderCreatedTopicSubscription);

    }

    public async Task Start()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailCartProcessor.StartProcessingAsync();

        _registeredUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
        _registeredUserProcessor.ProcessErrorAsync += ErrorHandler;
        await _registeredUserProcessor.StartProcessingAsync();

        _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived ;
        _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailOrderPlacedProcessor.StartProcessingAsync();

    }

    private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        var rewards = JsonConvert.DeserializeObject<RewardsMessage>(body)!;
        try
        {
            //TODO - try to log email
            await _emailService.LogOrderPlaced(rewards);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        string email = JsonConvert.DeserializeObject<string>(body)!;
        try
        {
            //TODO - try to log email
            await _emailService.RegisterUserEmailAndLog(email);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task Stop()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();

        await _registeredUserProcessor.StopProcessingAsync();
        await _registeredUserProcessor.DisposeAsync();

        await _emailOrderPlacedProcessor.StopProcessingAsync();
        await _emailOrderPlacedProcessor.DisposeAsync();
    }

    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body)!;
        try
        {
            //TODO - try to log email
            await _emailService.EmailCartAndLog(objMessage);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }

    }


    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }


}
