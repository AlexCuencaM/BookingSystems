using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using BookingAPI.Core.Models.Entities;
using BookingAPI.Core.Models.Interfaces;

namespace BookingAPI.Core.Models.DTOs;

public class BookingRequestDTO: IRequestDTO<Booking>
{
    public DateTime FromDate { get; set; }
    [BsonExtraElements]
    public BsonDocument? DynamicProperties { get; set; }
    public Booking ToEntity()
    {
        Booking booking = new()
        {
            FromDate = FromDate,
            DynamicProperties = DynamicProperties
        };
        //if(DynamicProperties is not null)
        //{
        //    booking.DynamicProperties = DynamicProperties;
        //}
        return booking;
    }
}
