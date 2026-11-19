using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public static void Map(RouteGroupBuilder group) => group.MapGet("/{id:guid}", Handle)
        .WithName("GetRoomById");

    private static IResult Handle(Guid id, RoomService service)
    {
        var room = service.Get(id);
        return Results.Ok(new RoomResponse(room.Id, room.Name, room.Capacity));
    }
}
