using FluentValidation;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Feeds.Commands.CreateFeed
{
    public class CreateFeedCommandValidator : AbstractValidator<CreateFeedCommand>
    {
        public CreateFeedCommandValidator(IApplicationDbContext context)
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Feed name is required.")
                .MaximumLength(100).WithMessage("Feed text must not exceed 100 characters.");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Feed text must not exceed 500 characters");
        }
    }
}
