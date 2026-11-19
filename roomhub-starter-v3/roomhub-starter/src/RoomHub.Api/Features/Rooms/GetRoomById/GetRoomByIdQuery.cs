using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public sealed record GetRoomByIdQuery(Guid Id) : IQuery<Result<Room>>;

public sealed class GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    : IQueryHandler<GetRoomByIdQuery, Result<Room>>
{
    public async Task<Result<Room>> HandleAsync(GetRoomByIdQuery query, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(query.Id, cancellationToken);
        if (room is null)
        {
            return Result<Room>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist."));
        }

        return Result<Room>.Success(room);
    }
}
