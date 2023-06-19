namespace Turnit.GenericStore.Data.Values
{
    public class ProductCategoryItem
    {
        public Guid? CategoryId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = default!;

        public Guid StoreId { get; set; }

        public int Availability { get; set; }
    }
}
