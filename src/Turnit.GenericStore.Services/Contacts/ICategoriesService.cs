using Turnit.GenericStore.Data.Models;

namespace Turnit.GenericStore.Services.Contacts
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync(CancellationToken token = default);
    }
}
