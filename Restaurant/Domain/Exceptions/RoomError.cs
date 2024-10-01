using SharedLib.Models.Common;

namespace Domain.Exceptions;

public sealed class RoomError
{
    public static ApiOperationError RoomName() => ApiOperationError.Validation(nameof(RoomName),"Room name is required");
    public static ApiOperationError RoomWithWaiterInChargeNull(int roomId) => ApiOperationError.Validation(nameof(RoomWithWaiterInChargeNull), $"The room with id:{roomId} has no waiter in charge");
    public static ApiOperationError RoomWaiterInChargeNotFound(int waiterId) => ApiOperationError.NotFound(nameof(RoomWaiterInChargeNotFound), $"The waiter in charge of the room with id:{waiterId} doesn't exist");
    public static ApiOperationError WaiterInChargeWithRoom(string Name) => ApiOperationError.Validation(nameof(WaiterInChargeWithRoom), $"The waiter: {Name} is already in charge of a room and a waiter cannot be in charge of more than one room");
    public static ApiOperationError InvalidId(int id) => ApiOperationError.Validation(nameof(InvalidId), $"The room with id: {id} is Invalid");
    public static ApiOperationError RoomNameDuplicated(string name) => ApiOperationError.Conflict(nameof(RoomNameDuplicated), $"A room already has the name {name} and cannot be duplicated");
    public static ApiOperationError NotFound(int id) => ApiOperationError.NotFound(nameof(NotFound), $"The room with id: {id} was not found");
    public static ApiOperationError RoomWithTables(string name) => ApiOperationError.Validation(nameof(RoomWithTables), $"The room with name: {name} still has tables");


}
