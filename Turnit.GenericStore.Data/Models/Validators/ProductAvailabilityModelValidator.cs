// Ignore Spelling: Validator Validators Turnit

using FluentValidation;

namespace Turnit.GenericStore.Data.Models.Validators
{
    public class ProductAvailabilityModelValidator : AbstractValidator<ProductAvailabilityModel>
    {
        public ProductAvailabilityModelValidator() 
        {
            RuleFor(model => model.StoreId).NotEmpty();
            RuleFor(model => model.Availability).GreaterThanOrEqualTo(0);
        }
    }
}
