namespace BookingAPI.Core.Models.DTOs;

public class RabbitMQCredentials
{
    public string HostName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
