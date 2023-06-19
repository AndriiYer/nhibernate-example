using FluentNHibernate.Mapping;
using NHibernate;
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

            using var transaction = Session.BeginTransaction();
            var producteCategory = new ProductCategory
            {
                Id = Guid.NewGuid(),
                Category = category,
                Product = product
            };

            await Session.SaveAsync(producteCategory, token);
            await transaction.CommitAsync(token);
        }

        public async Task RemoveProductFromCategoryAsync(Guid categoryId, Guid productId, CancellationToken token = default)
        {
            var productCategory = await GetProductCategory(categoryId, productId, token);
            if (productCategory == null)
            {
                throw new ArgumentException("Provided wrong arguments");
            }

            using var transaction = Session.BeginTransaction();
            await Session.DeleteAsync(productCategory, token);
            await transaction.CommitAsync(token);
        }

        /// <summary>
        /// This method was re-factored according the assignment
        /// </summary>
        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductsAsync(CancellationToken token = default)
        {
            var query = Session.CreateSQLQuery(@"
                SELECT  pc.category_id AS CategoryId, pa.id AS ProductId, pa.name AS ProductName, pa.store_id AS StoreId, pa.availability AS Availability
                FROM
                    (SELECT p.id, p.name, a.store_id, a.availability
                    FROM product p
                    LEFT JOIN product_availability a
                    ON p.id = a.product_id
                    ) AS pa
                LEFT JOIN product_category pc
                ON pa.id = pc.product_id
                ").SetResultTransformer(Transformers.AliasToBean<ProductCategoryItem>());

            var result = (await query.ListAsync<ProductCategoryItem>(token))
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

            using var transaction = Session.BeginTransaction();
            await Session.UpdateAsync(productAvailability, token);
            await transaction.CommitAsync(token);
        }

        private async Task<ProductCategory> GetProductCategory(Guid categoryId, Guid productId, CancellationToken token) =>
            await Session.QueryOver<ProductCategory>()
                .Where(x => x.Category.Id == categoryId)
                .Where(x => x.Product.Id == productId)
                .SingleOrDefaultAsync(token);
    }
}
