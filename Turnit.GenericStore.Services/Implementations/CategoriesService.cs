using NHibernate;
using NHibernate.Transform;
using Turnit.GenericStore.Data.Entities;
using Turnit.GenericStore.Data.Models;
using Turnit.GenericStore.Services.Contacts;

namespace Turnit.GenericStore.Services.Implementations
{
    public class CategoriesService : ServiceBase, ICategoriesService
    {
        public CategoriesService(ISession session) : base(session) { }

        /// <summary>
        /// This method was moved and re-factored
        /// </summary>
        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync(CancellationToken token = default)
        {
            var res = await Session
                .QueryOver<Category>()
                .ListAsync();

            Category categoryAlias = default!;
            CategoryModel categoryModelAlias = default!;

            var categories = await Session
                .QueryOver<Category>(() => categoryAlias)
                .SelectList(list => list
                    .Select(() => categoryAlias.Id).WithAlias(() => categoryModelAlias.Id)
                    .Select(() => categoryAlias.Name).WithAlias(() => categoryModelAlias.Name))
                .TransformUsing(Transformers.AliasToBean<CategoryModel>())
                .ListAsync<CategoryModel>(token);

            return categories;
        }
    }
}
