using FluentValidation;

namespace Barebones.Pages.Validation
{
	class ShellViewModelValidator : AbstractValidator<ShellViewModel>
	{
		public ShellViewModelValidator()
		{
			RuleFor(m => m.Width).NotEmpty();

			RuleFor(m => m.Height).NotEmpty();

			RuleFor(m => m.Width).Must(w =>
			{
				uint temp;
				return uint.TryParse(w, out temp);
			}).WithMessage("Width must be a positive integer.");

			RuleFor(m => m.Height).Must(w =>
			{
				uint temp;
				return uint.TryParse(w, out temp);
			}).WithMessage("Height must be a positive integer.");
		}
	}
}
