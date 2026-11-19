using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public sealed record BookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

public static class GetBookingByIdEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", Handle)
        .WithName("GetBookingById");

    private static IResult Handle(Guid id, BookingService service)
    {
        var b = service.Get(id);
        return Results.Ok(new BookingResponse(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
    }
}
