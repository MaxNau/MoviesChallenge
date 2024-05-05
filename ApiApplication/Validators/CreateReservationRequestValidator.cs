using ApiApplication.Contracts;
using ApiApplication.Extension;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace ApiApplication.Validators
{
    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor(m => m.Seats)
                .Must(m => m.IsSeatsContiguous())
                .WithMessage("All seats should be contiguous");

            RuleFor(m => m.Seats)
                .Must(m => m.All(s => s.Row > 0))
                .WithMessage("Seat rows should be positive integers only");

            RuleFor(m => m.Seats)
                .Must(m => m.All(s => s.SeatNumber > 0))
                .WithMessage("Seat numbers should be positive integers only");

            RuleFor(m => m.ShowtimeId)
                .NotEmpty()
                .WithMessage($"{nameof(CreateReservationRequest.ShowtimeId)} is required");

            RuleFor(m => m.Seats)
                .NotEmpty()
                .WithMessage("At least one seat needs to be reserved");
        }

        protected override bool PreValidate(ValidationContext<CreateReservationRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CreateReservationRequest), "Please ensure a model was supplied."));
                return false;
            }

            if (context.InstanceToValidate.Seats == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CreateReservationRequest.Seats), "Please ensure a model was supplied."));
                return false;
            }

            return true;
        }
    }
}
