// Ignore Spelling: Validator Validators Turnit

using FluentValidation;

namespace Turnit.GenericStore.Data.Models.Validators
{
    public class ProductRestockModelValidator : AbstractValidator<ProductRestockModel>
    {
        public ProductRestockModelValidator() 
        {
            RuleFor(model => model.ProductId).NotEmpty();
            RuleFor(model => model.Count).GreaterThanOrEqualTo(0);
        }
    }
}
