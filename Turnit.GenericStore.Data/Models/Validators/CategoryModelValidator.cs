// Ignore Spelling: Validator Validators Turnit

using FluentValidation;

namespace Turnit.GenericStore.Data.Models.Validators
{
    public class CategoryModelValidator : ModelBaseValidator<CategoryModel>
    {
        public CategoryModelValidator()
        {
            RuleFor(model => model.Name).NotEmpty();
        }
    }
}
