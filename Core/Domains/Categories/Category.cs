using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Cvs;
using Core.Domains.Jobs;

namespace Core.Domains.Categories;

public class Category : IEntityWithId
{
    public static readonly ImmutableArray<Category> AllValues =
    [
        new Category(1)
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];

    private Category(long id)
    {
        Id = id;
    }
    
    public long Id { get; }
    
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
}