namespace RoomHub.Api.Domain.Common;

public sealed class Unit
{
    public static readonly Unit Value = new();
    private Unit() { }
}
