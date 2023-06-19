using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Transactions;
using Turnit.GenericStore.Data.Entities;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Data.Values;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Services.Implementations
{
    public class ProductsService : ServiceBase, IProductsService
    {

        public ProductsService(ISession session) : base(session) { }

        public async Task AddProductToCategoryAsync(Guid categoryId, Guid productId, CancellationToken token = default)
        {
            var productCategory = await GetProductCategory(categoryId, productId, token);
            if (productCategory != null)
            {
                throw new ArgumentException("Provided wrong arguments. The product is already in the category");
            }

            var product = await Session.GetAsync<Product>(productId);
            if (product == null)
            {
                throw new ArgumentException("Provided wrong productId");
            }

            var category = await Session.GetAsync<Category>(categoryId);
            if (product == null)
            {
                throw new ArgumentException("Provided wrong categoryId");
            }

            var producteCategory = new ProductCategory
            {
                Id = Guid.NewGuid(),
                Category = category,
                Product = product
            };

            await Session.SaveAsync(producteCategory, token);
            await Session.FlushAsync(token);
        }

        public async Task RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token = default)
        {
            var productCategory = await GetProductCategory(categoryId, productId, token);
            if (productCategory == null)
            {
                throw new ArgumentException("Provided wrong arguments");
            }

            await Session.DeleteAsync(productCategory, token);
            await Session.FlushAsync(token);
        }

        /// <summary>
        /// This method was re-factored according the assignment
        /// </summary>
        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductsAsync(CancellationToken token = default)
        {
            var query = Session
                .Query<Product>()
                .LeftJoin(
                    Session.Query<ProductAvailability>(),
                    product => product.Id,
                    productAvailability => productAvailability.Product.Id,
                    (product, productAvailability) => new
                    {
                        Product = product,
                        Availability = productAvailability
                    })
                .LeftJoin(
                    Session.Query<ProductCategory>(),
                    product => product.Product.Id,
                    productCategory => productCategory.Product.Id,
                    (product, productCategory) => new ProductCategoryItem
                    {
                        ProductId = product.Product.Id,
                        ProductName = product.Product.Name,
                        StoreId = product.Availability.Store.Id,
                        Availability = product.Availability.Availability,
                        CategoryId = productCategory.Category == null ? null : productCategory.Category.Id
                    });

            var result = (await query.ToListAsync())
                .GroupBy(x => new { x.ProductId, x.ProductName, x.CategoryId })
                .Select(x => new
                {
                    CategoryId = x.Key.CategoryId,
                    Product = new ProductModel
                    {
                        Id = x.Key.ProductId,
                        Name = x.Key.ProductName,
                        Availability = x.Select(p => new ProductAvailabilityModel
                        {
                            StoreId = p.StoreId,
                            Availability = p.Availability
                        })
                    }

                })
                .GroupBy(x => x.CategoryId)
                .Select(x => new ProductCategoryModel
                {
                    CategoryId = x.Key,
                    Products = x.Select(x => x.Product)
                });

            return result;
        }

        /// <summary>
        /// This method was moved as is
        /// </summary>
        public async Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken token = default)
        {
            var products = await Session.QueryOver<ProductCategory>()
            .Where(x => x.Category.Id == categoryId)
            .Select(x => x.Product)
            .ListAsync<Product>(token);

            var result = new List<ProductModel>();

            foreach (var product in products)
            {
                var availability = await Session.QueryOver<ProductAvailability>()
                    .Where(x => x.Product.Id == product.Id)
                    .ListAsync(token);

                var model = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Availability = availability.Select(x => new ProductAvailabilityModel
                    {
                        StoreId = x.Store.Id,
                        Availability = x.Availability
                    }).ToArray()
                };
                result.Add(model);
            }

            return result.ToArray();
        }

        public async Task BookProductAsync(Guid productId, ProductBookingModel productBookingModel, CancellationToken token = default)
        {
            var productAvailability = await Session.QueryOver<ProductAvailability>()
                .Where(x => x.Store.Id == productBookingModel.StoreId)
                .Where(x => x.Product.Id == productId)
                .SingleOrDefaultAsync(token);

            if (productAvailability == null)
            {
                throw new ArgumentException("Provided wrong arguments");
            }

            if(productAvailability.Availability < productBookingModel.Count)
            {
                throw new ArgumentException("Product availability is less than booking count");
            }

            productAvailability.Availability = productAvailability.Availability - productBookingModel.Count;

            await Session.UpdateAsync(productAvailability, token);
            await Session.FlushAsync(token);
        }

        private async Task<ProductCategory> GetProductCategory(Guid categoryId, Guid productId, CancellationToken token) =>
            await Session.QueryOver<ProductCategory>()
                .Where(x => x.Category.Id == categoryId)
                .Where(x => x.Product.Id == productId)
                .SingleOrDefaultAsync(token);
    }
}
