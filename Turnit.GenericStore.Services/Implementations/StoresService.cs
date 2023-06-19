using NHibernate;
using Turnit.GenericStore.Data.Entities;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Services.Implementations
{
    public class StoresService : ServiceBase, IStoresService
    {
        public StoresService(ISession session) : base(session) { }

        public async Task RestockProductAsync(Guid storeId, ProductRestockModel productRestockModel, CancellationToken token = default)
        {
            var productAvailability = await Session.QueryOver<ProductAvailability>()
                .Where(x => x.Store.Id ==storeId)
                .Where(x => x.Product.Id == productRestockModel.ProductId)
                .SingleOrDefaultAsync(token);

            if (productAvailability == null)
            {
                throw new ArgumentException("Provided wrong arguments");
            }

            productAvailability.Availability = productAvailability.Availability + productRestockModel.Quantity;

            await Session.UpdateAsync(productAvailability, token);
            await Session.FlushAsync(token);
        }
    }
}
