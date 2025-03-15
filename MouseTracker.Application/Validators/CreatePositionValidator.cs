using FluentValidation;
using MouseTracker.Application.DTOs.Request;

namespace MouseTracker.Application.Validators
{
    public class CreatePositionValidator : AbstractValidator<CreatePositionDto>
    {
        public CreatePositionValidator()
        {
            RuleFor(p => p.X).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Y).GreaterThanOrEqualTo(0);
        }
    }

    public class CreatePositionsValidator : AbstractValidator<CreatePositionDto[]>
    {
        public CreatePositionsValidator()
        {
            RuleFor(p => p).NotEmpty();
            RuleForEach(p => p).SetValidator(new CreatePositionValidator());
        }
    }
}
