using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<Room>;

public sealed class CreateRoomHandler(RoomRepository roomRepository)
    : ICommandHandler<CreateRoomCommand, Room>
{
    public Task<Result<Room>> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (roomResult.IsFailure)
        {
            return Task.FromResult(Result<Room>.Failure(roomResult.Error));
        }

        roomRepository.Add(roomResult.Value);
        return Task.FromResult(Result<Room>.Success(roomResult.Value));
    }
}
