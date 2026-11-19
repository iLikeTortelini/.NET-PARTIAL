using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public sealed record RoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapPost("/", Handle)
        .WithName("CreateRoom");

    private static IResult Handle(CreateRoomRequest request, RoomService service)
    {
        var room = service.Create(request.Name, request.Capacity);
        return Results.Created(
            $"/api/rooms/{room.Id}",
            new RoomResponse(room.Id, room.Name, room.Capacity));
    }
}
