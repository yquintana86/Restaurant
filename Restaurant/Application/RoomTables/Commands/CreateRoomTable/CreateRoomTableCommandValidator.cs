using FluentValidation;

namespace Application.RoomTable.Commands.CreateTable;

public class CreateRoomTableCommandValidator : AbstractValidator<CreateRoomTableCommand>
{
    public CreateRoomTableCommandValidator()
    {
        RuleFor(ct => ct.RoomId).GreaterThan(0);
        RuleFor(ct => ct.TotalQty).GreaterThanOrEqualTo(2);
        RuleFor(ct => ct.WaiterId).Custom((id, ctx) => 
        {
            if (id is not null && id == 0)
                ctx.AddFailure(nameof(id), "Invalid WaiterId");
        });
    }
}