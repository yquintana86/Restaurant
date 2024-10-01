using SharedLib.Models.Common;
using System.Globalization;
using System.Text;

namespace Domain.Exceptions;

public sealed class WaiterError
{
    public static ApiOperationError NotFound(int id) => ApiOperationError.NotFound(nameof(NotFound), $"The waiter with id: {id} was not found");
    public static ApiOperationError WaiterIdInvalid() => ApiOperationError.Validation(nameof(WaiterIdInvalid), $"The waiter id requested is invalid");
    public static ApiOperationError WaiterInChargeCannotBeDeleted() => ApiOperationError.Validation(nameof(WaiterInChargeCannotBeDeleted), $"A waiter in charge of a room cannot be eliminated");
     
}
