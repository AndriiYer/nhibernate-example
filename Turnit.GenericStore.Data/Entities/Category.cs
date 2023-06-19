using FluentNHibernate.Mapping;

namespace Turnit.GenericStore.Data.Entities;

public class Category //: EnityBase
{
    public virtual Guid Id { get; set; }

    public virtual string Name { get; set; } = default!;
}

public class CategoryMap : ClassMap<Category>
{
    public CategoryMap()
    {
        Schema("public");
        Table("category");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
    }
}