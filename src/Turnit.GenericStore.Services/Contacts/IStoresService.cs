using Turnit.GenericStore.Data.Models;

namespace Turnit.GenericStore.Services.Contacts
{
    public interface IStoresService
    {
        Task RestockProductAsync(Guid storeId, ProductRestockModel productRestockModel, CancellationToken token = default);
    }
}
