using FluentNHibernate.Mapping;

namespace Turnit.GenericStore.Data.Entities;

public class Store
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; } = default!;
}

public class StoreMap : ClassMap<Store>
{
    public StoreMap()
    {
        Schema("public");
        Table("product");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
    }
}