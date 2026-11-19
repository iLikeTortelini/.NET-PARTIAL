using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/{id:guid}/cancel", Handle)
        .WithName("CancelBooking");

    private static async Task<IResult> Handle(
        Guid id,
        ICommandHandler<CancelBookingCommand, Unit> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CancelBookingCommand(id), cancellationToken);
        if (result.IsFailure)
        {
            return ProblemResults.ToProblem(result.Error);
        }

        return Results.NoContent();
    }
}
