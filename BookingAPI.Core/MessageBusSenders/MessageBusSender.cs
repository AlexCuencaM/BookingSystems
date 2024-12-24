using BookingAPI.Core.Models.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
namespace BookingAPI.Core.MessageBusSenders;

public class MessageBusSender(IOptions<RabbitMQCredentials> credentialsOptions) : IMessageBusSender
{
    private RabbitMQCredentials Credentials = credentialsOptions.Value;
    private IConnection _connection = null!;
    public async Task SendMessageAsync(object message, string queueName)
    {
        bool existsConnection = await ConnectionExists();
        if (existsConnection)
        {
            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, false, false, false, null);
            var json = JsonConvert.SerializeObject(message);
            var body2 = Encoding.UTF8.GetBytes(json);
            //var memory = new ReadOnlyMemory<byte>(body2);
            await channel.BasicPublishAsync(exchange:string.Empty, routingKey: queueName, body: body2, mandatory: true);
        }
    }
    private async Task CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = Credentials.HostName,
                Password = Credentials.Password,
                UserName = Credentials.UserName
            };
            _connection = await factory.CreateConnectionAsync();
        }
        catch
        {

        }
    }

    private async Task<bool> ConnectionExists()
    {
        if(_connection is not null)
        {
            return true;
        }
        await CreateConnection();
        return true;
    }
}
