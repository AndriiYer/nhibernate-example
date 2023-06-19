// Ignore Spelling: Validator Validators Turnit

using FluentValidation;

namespace Turnit.GenericStore.Data.Models.Validators
{
    public class ModelBaseValidator<T> : AbstractValidator<T> where T : ModelBase
    {
        public ModelBaseValidator()
        {
            RuleFor(modelBase => modelBase.Id).NotEmpty();
        }
    }
}
