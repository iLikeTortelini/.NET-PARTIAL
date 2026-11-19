using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBookingById;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoomById;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("RoomHubDb") ?? "Data Source=roomhub.db";

builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RoomHubDbContext>());
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/", () => "RoomHub API is running.");

var rooms = app.MapGroup("/api/rooms").WithTags("Rooms");
CreateRoomEndpoint.Map(rooms);
GetRoomByIdEndpoint.Map(rooms);
CreateBookingEndpoint.Map(rooms);

var bookings = app.MapGroup("/api/bookings").WithTags("Bookings");
GetBookingByIdEndpoint.Map(bookings);
CancelBookingEndpoint.Map(bookings);

app.Run();
