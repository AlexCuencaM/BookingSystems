using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BookingAPI.Core.Models.Entities;

public class Booking
{
    [BsonId]
    [BsonElement("_id")]
    public ObjectId Id { get; set; }
    public DateTime FromDate { get; set; }
    [BsonExtraElements]
    public BsonDocument? DynamicProperties { get; set; }
    public override string ToString()
    {
        return $"Booking creado exitosamente";
        
    }
}
