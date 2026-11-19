using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/{id:guid}/cancel", Handle)
        .WithName("CancelBooking");

    private static IResult Handle(Guid id, BookingService service)
    {
        service.Cancel(id);
        return Results.NoContent();
    }
}
