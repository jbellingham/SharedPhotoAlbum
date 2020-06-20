using FluentValidation;
using SharedPhotoAlbum.Application.Common.Interfaces;

namespace SharedPhotoAlbum.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCommentCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Text)
                .NotEmpty().WithMessage("Comment text is required.")
                .MaximumLength(1000).WithMessage("Comment text must not exceed 1000 characters.");
            // .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
        }

        // public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        // {
        //     return await _context.Posts
        //         .AllAsync(l => l.Title != title);
        // }
    }
}