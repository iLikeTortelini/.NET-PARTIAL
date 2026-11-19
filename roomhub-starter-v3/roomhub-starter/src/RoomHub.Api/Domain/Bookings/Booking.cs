using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking()
    {
    }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = null!;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidBookedBy", "BookedBy is required."));
        }

        if (endUtc <= startUtc)
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidRange", "The end must be after the start."));
        }

        if (attendees <= 0)
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidAttendees", "Attendees must be positive."));
        }

        if (attendees > room.Capacity)
        {
            return Result<Booking>.Failure(
                Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats."));
        }

        if (startUtc < nowUtc)
        {
            return Result<Booking>.Failure(
                Error.Validation("BookingInPast", "A booking cannot start in the past."));
        }

        if (endUtc - startUtc > MaxDuration)
        {
            return Result<Booking>.Failure(
                Error.Validation("BookingTooLong", "The maximum duration of a booking is 4 hours."));
        }

        return Result<Booking>.Success(new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        });
    }

    public bool OverlapsWith(Booking other)
    {
        if (Status != BookingStatus.Active || other.Status != BookingStatus.Active)
        {
            return false;
        }

        return other.StartUtc < EndUtc && StartUtc < other.EndUtc;
    }

    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
        {
            return Result<Unit>.Failure(
                Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled."));
        }

        Status = BookingStatus.Cancelled;
        return Result<Unit>.Success(Unit.Value);
    }
}
