using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

public sealed record BookingCreatedResponse(Guid Id, Guid RoomId, string Status);

public static class CreateBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/{roomId:guid}/bookings", Handle)
        .WithName("CreateBooking");

    private static async Task<IResult> Handle(
        Guid roomId,
        CreateBookingRequest request,
        ICommandHandler<CreateBookingCommand, Booking> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            roomId,
            request.BookedBy,
            request.StartUtc,
            request.EndUtc,
            request.Attendees);

        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
        {
            return ProblemResults.ToProblem(result.Error);
        }

        var booking = result.Value;
        return Results.Created(
            $"/api/bookings/{booking.Id}",
            new BookingCreatedResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}
