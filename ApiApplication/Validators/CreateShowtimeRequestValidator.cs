using ApiApplication.Contracts;
using FluentValidation;
using FluentValidation.Results;

namespace ApiApplication.Validators
{
    public class CreateShowtimeRequestValidator : AbstractValidator<CreateShowtimeRequest>
    {
        public CreateShowtimeRequestValidator()
        {
            RuleFor(m => m.AuditoriumId)
                .NotEmpty()
                .WithMessage($"{nameof(CreateShowtimeRequest.AuditoriumId)} is required");

            RuleFor(m => m.SessionDate)
                .NotEmpty()
                .WithMessage($"{nameof(CreateShowtimeRequest.SessionDate)} is required");

            RuleFor(m => m.Movie.ImdbId)
                .NotEmpty()
                .WithMessage($"{nameof(CreateShowtimeRequest.Movie.ImdbId)} is required");
        }

        protected override bool PreValidate(ValidationContext<CreateShowtimeRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CreateShowtimeRequest), "Please ensure a model was supplied."));
                return false;
            }

            if (context.InstanceToValidate.Movie == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CreateShowtimeRequest.Movie), "Please ensure a model was supplied."));
                return false;
            }

            return true;
        }
    }
}
