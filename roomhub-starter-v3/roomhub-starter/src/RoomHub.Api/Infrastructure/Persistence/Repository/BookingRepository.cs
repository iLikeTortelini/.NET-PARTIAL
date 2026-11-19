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

    public async Task<int> CountActiveForParticipantOnDayAsync(string bookedBy, DateTime utcDate, CancellationToken cancellationToken = default)
    {
        var dayStart = DateTime.SpecifyKind(utcDate.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        return await dbContext.Bookings
            .CountAsync(b =>
                b.BookedBy == bookedBy &&
                b.Status == BookingStatus.Active &&
                b.StartUtc >= dayStart &&
                b.StartUtc < dayEnd,
                cancellationToken);
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        dbContext.Bookings.Add(booking);
        return Task.CompletedTask;
    }
}
