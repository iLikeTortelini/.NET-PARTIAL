using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common.CQRS;

public static class ProblemResults
{
    public static IResult ToProblem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        return Results.Problem(
            detail: error.Message,
            statusCode: statusCode,
            title: error.Code);
    }
}
