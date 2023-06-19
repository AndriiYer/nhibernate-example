using Microsoft.Extensions.DependencyInjection;
using Turnit.GenericStore.Services.Contacts;
using Turnit.GenericStore.Services.Implementations;

namespace Turnit.GenericStore.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IStoresService, StoresService>();

            return services;
        }
    }
}
