using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid BookingId) : ICommand<Unit>;

public sealed class CancelBookingHandler(BookingRepository bookingRepository)
    : ICommandHandler<CancelBookingCommand, Unit>
{
    public Task<Result<Unit>> HandleAsync(CancelBookingCommand command, CancellationToken cancellationToken = default)
    {
        var booking = bookingRepository.Get(command.BookingId);
        if (booking is null)
        {
            return Task.FromResult(Result<Unit>.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist.")));
        }

        var cancelResult = booking.Cancel();
        if (cancelResult.IsFailure)
        {
            return Task.FromResult(Result<Unit>.Failure(cancelResult.Error));
        }

        return Task.FromResult(Result<Unit>.Success(Unit.Value));
    }
}
