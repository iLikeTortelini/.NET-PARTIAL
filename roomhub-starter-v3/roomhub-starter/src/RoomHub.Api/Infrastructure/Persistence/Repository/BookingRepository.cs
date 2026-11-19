using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repository;

public sealed class BookingRepository(RoomHubDbContext dbContext) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> ListForRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bookings
            .Where(b => b.RoomId == roomId)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        dbContext.Bookings.Add(booking);
        return Task.CompletedTask;
    }
}
