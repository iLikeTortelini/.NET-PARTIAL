using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBookingById;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoomById;
using RoomHub.Api.Middleware;
using RoomHub.Api.Repositories;
using RoomHub.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BookingService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapGet("/", () => "RoomHub API is running.");

var rooms = app.MapGroup("/api/rooms").WithTags("Rooms");
CreateRoomEndpoint.Map(rooms);
GetRoomByIdEndpoint.Map(rooms);
CreateBookingEndpoint.Map(rooms);

var bookings = app.MapGroup("/api/bookings").WithTags("Bookings");
GetBookingByIdEndpoint.Map(bookings);
CancelBookingEndpoint.Map(bookings);

app.Run();
