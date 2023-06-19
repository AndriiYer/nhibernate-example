namespace Turnit.GenericStore.Data.Models
{
    public class ProductCategoryModel
    {
        public Guid? CategoryId { get; set; }

        public IEnumerable<ProductModel>? Products { get; set; }
    }
}
