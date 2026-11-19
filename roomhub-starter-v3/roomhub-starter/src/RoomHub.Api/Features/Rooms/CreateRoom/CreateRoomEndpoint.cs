using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public sealed record RoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/", Handle)
        .WithName("CreateRoom");

    private static async Task<IResult> Handle(
        CreateRoomRequest request,
        ICommandHandler<CreateRoomCommand, Room> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CreateRoomCommand(request.Name, request.Capacity), cancellationToken);
        if (result.IsFailure)
        {
            return ProblemResults.ToProblem(result.Error);
        }

        var room = result.Value;
        return Results.Created(
            $"/api/rooms/{room.Id}",
            new RoomResponse(room.Id, room.Name, room.Capacity));
    }
}
