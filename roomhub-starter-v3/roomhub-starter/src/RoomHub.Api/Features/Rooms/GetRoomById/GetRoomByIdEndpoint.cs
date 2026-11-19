using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Rooms.CreateRoom;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", Handle)
        .WithName("GetRoomById");

    private static async Task<IResult> Handle(
        Guid id,
        IQueryHandler<GetRoomByIdQuery, Result<Room>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetRoomByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            return ProblemResults.ToProblem(result.Error);
        }

        var room = result.Value;
        return Results.Ok(new RoomResponse(room.Id, room.Name, room.Capacity));
    }
}
