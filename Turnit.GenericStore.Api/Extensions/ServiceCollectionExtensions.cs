using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using Microsoft.Extensions.DependencyInjection;
using Turnit.GenericStore.Services.Contacts;
using Turnit.GenericStore.Services.Implementations;
using Turnit.GenericStore.Data;
using NHibernate;

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

        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .Dialect<NHibernate.Dialect.PostgreSQL82Dialect>()
                    .ConnectionString(connectionString))
                .Mappings(x =>
                {
                    x.FluentMappings.AddFromAssemblyOf<EnityBase>();
                });

            services.AddSingleton(configuration.BuildSessionFactory());
            services.AddScoped(sp => sp
                .GetRequiredService<ISessionFactory>()
                .OpenSession());

            return services;
        }
    }
}
