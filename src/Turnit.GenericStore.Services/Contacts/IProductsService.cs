using Turnit.GenericStore.Data.Models;

namespace Turnit.GenericStore.Services.Contacts
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken token = default);

        Task<IEnumerable<ProductCategoryModel>> GetAllProductsAsync(CancellationToken token = default);

        Task<bool> AddProductToCategoryAsync(Guid categoryId, Guid productId, CancellationToken token);

        Task<bool> RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token);
    }
}
