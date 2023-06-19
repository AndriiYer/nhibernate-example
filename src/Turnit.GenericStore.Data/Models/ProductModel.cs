namespace Turnit.GenericStore.Data.Models;

public class ProductModel : ModelBase
{
    public string Name { get; set; } = default!;

    public IEnumerable<ProductAvailabilityModel>? Availability { get; set; }
}