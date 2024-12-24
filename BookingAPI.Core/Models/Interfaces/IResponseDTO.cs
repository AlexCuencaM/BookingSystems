namespace BookingAPI.Core.Models.Interfaces;

public interface IResponseDTO<Dto>
{
    Dto ToDto();
}
