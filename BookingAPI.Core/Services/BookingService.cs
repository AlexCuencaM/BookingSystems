using BookingAPI.Core.Models.DTOs;
using BookingAPI.Core.Models.Entities;
using BookingAPI.Core.Models.Interfaces;
using BookingAPI.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookingAPI.Core.Services;
public class BookingService : IBookingService
{
    private readonly IMongoCollection<BsonDocument> _bookingCollection;
    public BookingService(BookingsSystemDatabaseSettings options)
    {
        var mongoClient = new MongoClient(options.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.DatabaseName);
        _bookingCollection = mongoDatabase.GetCollection<BsonDocument>(
            options.Collections.Bookings);
    }
    public async Task<ServiceResponseDTO> CreateBookingAsync(BsonDocument document)
    {
        string date = document.GetValue("FromDate").AsString;
        bool isACorrectDate = DateTime.TryParse(date, out DateTime fromDate);
        if (isACorrectDate)
        {
            document.Set("FromDate",  fromDate.ToUniversalTime());
            await _bookingCollection.InsertOneAsync(document);
            return new()
            {
                Message = "Booking registered successfully"
            };
        }
        return new()
        {
            Message = "FromDate is required"
        };
    }

    public async Task<ServiceResponseDTO> GetBookingAsync()
    {
        //var filter = Builders<Booking>.Filter.Eq(_ => true);
        var results = await _bookingCollection.Find(_ => true).ToListAsync();
        return new ServiceResponseDTO
        {
            Data = results,
        };
    }
}
