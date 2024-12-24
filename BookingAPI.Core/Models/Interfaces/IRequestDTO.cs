namespace BookingAPI.Core.Models.Interfaces;

public interface IRequestDTO<Entity>
{
    Entity ToEntity();
}
