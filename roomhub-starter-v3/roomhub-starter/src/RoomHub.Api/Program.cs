using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBookingById;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoomById;
using RoomHub.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();
builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

app.MapGet("/", () => "RoomHub API is running.");

var rooms = app.MapGroup("/api/rooms").WithTags("Rooms");
CreateRoomEndpoint.Map(rooms);
GetRoomByIdEndpoint.Map(rooms);
CreateBookingEndpoint.Map(rooms);

var bookings = app.MapGroup("/api/bookings").WithTags("Bookings");
GetBookingByIdEndpoint.Map(bookings);
CancelBookingEndpoint.Map(bookings);

app.Run();
