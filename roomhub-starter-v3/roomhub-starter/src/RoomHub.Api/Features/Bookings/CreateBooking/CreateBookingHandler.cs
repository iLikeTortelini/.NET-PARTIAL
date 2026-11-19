using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<Booking>;

public sealed class CreateBookingHandler(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateBookingCommand, Booking>
{
    public async Task<Result<Booking>> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
        {
            return Result<Booking>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist."));
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
            return Result<Booking>.Failure(bookingResult.Error);
        }

        var newBooking = bookingResult.Value;
        var existingBookings = await bookingRepository.ListForRoomAsync(command.RoomId, cancellationToken);
        foreach (var existing in existingBookings)
        {
            if (newBooking.OverlapsWith(existing))
            {
                return Result<Booking>.Failure(
                    Error.Conflict("RoomOccupied", "The room is occupied in this interval."));
            }
        }

        await bookingRepository.AddAsync(newBooking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Booking>.Success(newBooking);
    }
}
