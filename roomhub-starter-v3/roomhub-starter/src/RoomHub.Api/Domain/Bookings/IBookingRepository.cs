namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> ListForRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<int> CountActiveForParticipantOnDayAsync(string bookedBy, DateTime utcDate, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
}
