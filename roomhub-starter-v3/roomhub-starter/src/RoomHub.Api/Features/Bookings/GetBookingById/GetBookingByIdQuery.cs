using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public sealed record GetBookingByIdQuery(Guid Id) : IQuery<Result<Booking>>;

public sealed class GetBookingByIdQueryHandler(BookingRepository bookingRepository)
    : IQueryHandler<GetBookingByIdQuery, Result<Booking>>
{
    public Task<Result<Booking>> HandleAsync(GetBookingByIdQuery query, CancellationToken cancellationToken = default)
    {
        var booking = bookingRepository.Get(query.Id);
        if (booking is null)
        {
            return Task.FromResult(Result<Booking>.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist.")));
        }

        return Task.FromResult(Result<Booking>.Success(booking));
    }
}
