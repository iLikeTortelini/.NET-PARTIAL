using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid Id) : IQuery<Result<Booking>>;

public sealed class GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingByIdQuery, Result<Booking>>
{
    public async Task<Result<Booking>> HandleAsync(GetBookingByIdQuery query, CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByIdAsync(query.Id, cancellationToken);
        if (booking is null)
        {
            return Result<Booking>.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist."));
        }

        return Result<Booking>.Success(booking);
    }
}
