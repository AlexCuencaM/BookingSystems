
using BookingAPI.Core.Models.DTOs;
using BookingAPI.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace BookingAPI.Core.Messaging;

public class BookingConsumer : BackgroundService
{

    private RabbitMQCredentials Credentials;
    private readonly IConfiguration _configuration;
    private readonly IBookingService _bookingService;
    private IConnection _connection;
    private string _queueName;
    private IChannel _channel;
    public BookingConsumer(IOptions<RabbitMQCredentials> credentialsOptions, IConfiguration configuration, IBookingService bookingService)
    {
        _configuration = configuration;
        _bookingService = bookingService;
        Credentials = credentialsOptions.Value;
        _queueName = _configuration.GetValue<string>("TopicAndQueueNames:BookingsQueue")
                ?? throw new InvalidOperationException("Appsettings key TopicAndQueueNames:BookingsQueue doesn't exist");
        var factory = new ConnectionFactory
        {
            HostName = Credentials.HostName,
            Password = Credentials.Password,
            UserName = Credentials.UserName
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.QueueDeclareAsync(_queueName, false, false, false, null).GetAwaiter().GetResult();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += Receives_Message_From_Queue;
        await _channel.BasicConsumeAsync(_queueName, false, consumer, cancellationToken: stoppingToken);
    }

    private async Task Receives_Message_From_Queue(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var content = Encoding.UTF8.GetString(@event.Body.ToArray());
            BsonDocument document = BsonDocument.Parse(content);
            if (document is not null) { 
                var response = await _bookingService.CreateBookingAsync(document);
                await _channel.BasicAckAsync(@event.DeliveryTag, false);
            }
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(ex);
        }
    }
}
