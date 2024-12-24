namespace BookingAPI.Core.Models.DTOs;

public class BookingsSystemDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public DatabaseCollections Collections = null!;
}

public class DatabaseCollections
{
    public string Bookings { get; set; } = null!;
}
