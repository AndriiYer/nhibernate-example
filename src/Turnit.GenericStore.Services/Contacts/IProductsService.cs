using Turnit.GenericStore.Data.Models;

namespace Turnit.GenericStore.Services.Contacts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken token = default);

        Task<IEnumerable<ProductCategoryModel>> GetAllProductsAsync(CancellationToken token = default);

        Task AddProductToCategoryAsync(Guid categoryId, Guid productId, CancellationToken token = default);

        Task RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token = default);

        Task BookProductAsync(Guid productId, ProductBookingModel productBookingModel, CancellationToken token = default);
    }
}
