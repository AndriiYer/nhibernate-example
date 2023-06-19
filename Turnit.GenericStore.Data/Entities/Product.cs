using FluentNHibernate.Mapping;

namespace Turnit.GenericStore.Data.Entities;

public class Product
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; } = default!;

    public virtual string Description { get; set; } = default!;
}

public class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        Schema("public");
        Table("product");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
        Map(x => x.Description, "description");
    }
}