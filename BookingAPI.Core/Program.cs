using BookingAPI.Core.Messaging;
using BookingAPI.Core.Models.DTOs;
using BookingAPI.Core.Services;
using BookingAPI.Core.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookingsSystemDatabaseSettings>(
    builder.Configuration.GetSection(nameof(BookingsSystemDatabaseSettings)));
builder.Services.Configure<RabbitMQCredentials>(
    builder.Configuration.GetSection(nameof(RabbitMQCredentials)));
BookingsSystemDatabaseSettings connection = builder.Configuration
        .GetSection(nameof(BookingsSystemDatabaseSettings))
        .Get<BookingsSystemDatabaseSettings>()
        ?? throw new Exception($"appsettings value: {nameof(BookingsSystemDatabaseSettings)} not found");
connection.Collections = builder.Configuration
        .GetSection($"{nameof(BookingsSystemDatabaseSettings)}:{nameof(connection.Collections)}")
        .Get<DatabaseCollections>() ?? throw new Exception($"appsettings value: {nameof(BookingsSystemDatabaseSettings)} not found");
builder.Services.AddSingleton<IBookingService>(new BookingService(connection));
builder.Services.AddHostedService<BookingConsumer>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
