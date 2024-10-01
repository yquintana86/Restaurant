using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class TableError
{
    public static ApiOperationError TableNotFound(int tableId, int roomId) => ApiOperationError.NotFound(nameof(TableNotFound), $"Table with tableId: {tableId} and roomId:{roomId}  not found");
    public static ApiOperationError DeleteTableWithReservation(int tableId, int roomId) => ApiOperationError.Validation(nameof(DeleteTableWithReservation), $"Table with tableId: {tableId} and roomId:{roomId} still has active reservation, cannot be deleted");

}
