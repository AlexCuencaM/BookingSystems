namespace BookingAPI.Core.MessageBusSenders;

public interface IMessageBusSender
{
    Task SendMessageAsync(object message, string queueName);
}
