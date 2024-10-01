using FluentValidation;

namespace Application.RoomTable.Commands.DeleteTable;

public class DeleteRoomTableCommandValidator : AbstractValidator<DeleteRoomTableCommand>
{
    public DeleteRoomTableCommandValidator()
    {
        RuleFor(t => t.Id).GreaterThan(0);
        RuleFor(t => t.RoomId).GreaterThan(0);
    }
}
