using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    private static readonly TimeSpan MaxStandardDuration = TimeSpan.FromHours(4);
    private static readonly TimeSpan MaxLargeRoomDuration = TimeSpan.FromHours(2);
    private static readonly TimeSpan MinCancelLeadTime = TimeSpan.FromMinutes(60);
    public const int MaxActiveBookingsPerDay = 2;

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

        var maxDuration = room.Capacity > 10 ? MaxLargeRoomDuration : MaxStandardDuration;
        if (endUtc - startUtc > maxDuration)
        {
            return Result<Booking>.Failure(
                Error.Validation("BookingTooLong", $"The maximum duration of a booking is {maxDuration.TotalHours} hours."));
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

    public static bool CanParticipantBookOnDay(int existingActiveBookingsOnDay)
    {
        return existingActiveBookingsOnDay < MaxActiveBookingsPerDay;
    }

    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
        {
            return Result<Unit>.Failure(
                Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled."));
        }

        if (StartUtc - nowUtc < MinCancelLeadTime)
        {
            return Result<Unit>.Failure(
                Error.Conflict("CancelTooLate", "A booking can be cancelled only if it starts at least 60 minutes from now."));
        }

        Status = BookingStatus.Cancelled;
        return Result<Unit>.Success(Unit.Value);
    }
}
