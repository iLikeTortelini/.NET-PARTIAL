using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repository;

public sealed class RoomRepository(RoomHubDbContext dbContext) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        dbContext.Rooms.Add(room);
        return Task.CompletedTask;
    }
}
