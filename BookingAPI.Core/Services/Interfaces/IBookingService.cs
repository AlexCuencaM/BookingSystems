using BookingAPI.Core.Models.DTOs;
using BookingAPI.Core.Models.Entities;
using BookingAPI.Core.Models.Interfaces;
using MongoDB.Bson;

namespace BookingAPI.Core.Services.Interfaces;

public interface IBookingService
{
    Task<ServiceResponseDTO> CreateBookingAsync(BsonDocument dto);
    Task<ServiceResponseDTO> GetBookingAsync();
}
