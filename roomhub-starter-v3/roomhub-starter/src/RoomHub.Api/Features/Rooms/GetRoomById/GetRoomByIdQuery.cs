using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public sealed record GetRoomByIdQuery(Guid Id) : IQuery<Result<Room>>;

public sealed class GetRoomByIdQueryHandler(RoomRepository roomRepository)
    : IQueryHandler<GetRoomByIdQuery, Result<Room>>
{
    public Task<Result<Room>> HandleAsync(GetRoomByIdQuery query, CancellationToken cancellationToken = default)
    {
        var room = roomRepository.Get(query.Id);
        if (room is null)
        {
            return Task.FromResult(Result<Room>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist.")));
        }

        return Task.FromResult(Result<Room>.Success(room));
    }
}
