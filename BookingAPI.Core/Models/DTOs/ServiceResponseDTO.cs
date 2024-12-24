namespace BookingAPI.Core.Models.DTOs;

public class ServiceResponseDTO
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}
