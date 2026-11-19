using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<Room>;

public sealed class CreateRoomHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Room>
{
    public async Task<Result<Room>> HandleAsync(CreateRoomCommand command, CancellationToken cancellationToken = default)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (roomResult.IsFailure)
        {
            return Result<Room>.Failure(roomResult.Error);
        }

        await roomRepository.AddAsync(roomResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Room>.Success(roomResult.Value);
    }
}
