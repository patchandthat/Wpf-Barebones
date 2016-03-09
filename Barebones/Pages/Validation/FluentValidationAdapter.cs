using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Barebones.Pages.Validation
{
	public class FluentValidationAdapter<T> : Stylet.IModelValidator<T>
	{
		private readonly IValidator<T> _validator;
		private T _subject;

		public FluentValidationAdapter(IValidator<T> validator)
		{
			this._validator = validator;
		}

		public void Initialize(object subject)
		{
			this._subject = (T)subject;
		}

		public async Task<IEnumerable<string>> ValidatePropertyAsync(string propertyName)
		{
			// If someone's calling us synchronously, and ValidationAsync does not complete synchronously,
			// we'll deadlock unless we continue on another thread.
			return (await this._validator.ValidateAsync(this._subject, propertyName).ConfigureAwait(false))
				 .Errors.Select(x => x.ErrorMessage);
		}

		public async Task<Dictionary<string, IEnumerable<string>>> ValidateAllPropertiesAsync()
		{
			// If someone's calling us synchronously, and ValidationAsync does not complete synchronously,
			// we'll deadlock unless we continue on another thread.
			return (await this._validator.ValidateAsync(this._subject).ConfigureAwait(false))
				 .Errors.GroupBy(x => x.PropertyName)
				 .ToDictionary(x => x.Key, x => x.Select(failure => failure.ErrorMessage));
		}
	}
}
