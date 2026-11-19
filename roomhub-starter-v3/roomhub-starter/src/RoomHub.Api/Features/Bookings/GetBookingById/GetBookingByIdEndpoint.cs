using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

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

    private static async Task<IResult> Handle(
        Guid id,
        IQueryHandler<GetBookingByIdQuery, Result<Booking>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetBookingByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            return ProblemResults.ToProblem(result.Error);
        }

        var b = result.Value;
        return Results.Ok(new BookingResponse(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
    }
}
