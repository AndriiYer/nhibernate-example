// Ignore Spelling: Validator Validators Turnit

using FluentValidation;

namespace Turnit.GenericStore.Data.Models.Validators
{
    public class ProductBookingModelValidator : AbstractValidator<ProductBookingModel>
    {
        public ProductBookingModelValidator() 
        {
            RuleFor(model => model.StoreId).NotEmpty();
            RuleFor(model => model.Quantity).GreaterThanOrEqualTo(0);
        }
    }
}
