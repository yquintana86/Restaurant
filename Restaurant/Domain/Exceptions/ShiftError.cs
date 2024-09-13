using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class ShiftError
{
    public static ApiOperationError ShiftNameInvalid() => ApiOperationError.Validation(nameof(ShiftNameInvalid),"Shift name is invalid");
    public static ApiOperationError ShiftNameExist(string name) => ApiOperationError.Conflict(nameof(ShiftNameExist), $"The shift with name: '{name}' already exist");
    public static ApiOperationError ShiftHasWaiters() => ApiOperationError.Validation(nameof(ShiftHasWaiters), $"A shift with waiters in it cannot be deleted");
    public static ApiOperationError NotFound(int id) => ApiOperationError.NotFound(nameof(NotFound), $"The shift with id: {id} was not found");
}
