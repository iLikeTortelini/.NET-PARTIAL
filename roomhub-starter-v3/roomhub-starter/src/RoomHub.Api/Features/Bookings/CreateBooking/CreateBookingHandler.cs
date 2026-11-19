using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<Booking>;

public sealed class CreateBookingHandler(
    RoomRepository roomRepository,
    BookingRepository bookingRepository,
    TimeProvider timeProvider) : ICommandHandler<CreateBookingCommand, Booking>
{
    public Task<Result<Booking>> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        var room = roomRepository.Get(command.RoomId);
        if (room is null)
        {
            return Task.FromResult(Result<Booking>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist.")));
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var bookingResult = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            nowUtc);

        if (bookingResult.IsFailure)
        {
            return Task.FromResult(Result<Booking>.Failure(bookingResult.Error));
        }

        var newBooking = bookingResult.Value;
        foreach (var existing in bookingRepository.ListForRoom(command.RoomId))
        {
            if (newBooking.OverlapsWith(existing))
            {
                return Task.FromResult(Result<Booking>.Failure(
                    Error.Conflict("RoomOccupied", "The room is occupied in this interval.")));
            }
        }

        bookingRepository.Add(newBooking);
        return Task.FromResult(Result<Booking>.Success(newBooking));
    }
}
