using FluentValidation;
using ToDoList.Application.Dto.User;

namespace ToDoList.Application.Validators.User
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required")
                .NotNull().WithMessage("Refresh token cannot be null")
                .MinimumLength(32).WithMessage("Refresh token must be at least 32 characters long")
                .MaximumLength(500).WithMessage("Refresh token must not exceed 500 characters")
                .Matches("^[A-Za-z0-9+/=]+$").WithMessage("Refresh token contains invalid characters");
        }
    }
}
