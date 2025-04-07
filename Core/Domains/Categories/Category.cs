using System.Collections.Immutable;
using Core.Domains._Shared.EntityInterfaces;
using Core.Domains.Categories.Seed;
// using Core.Domains.Cvs;
using Core.Domains.Jobs;

namespace Core.Domains.Categories;

public class Category : IEntityWithId
{
    public static readonly ImmutableArray<Category> AllValues = CategorySeedValues.All;
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];

    public Category(long id, string namePl)
    {
        Id = id;
        NamePl = namePl;
    }
    
    public long Id { get; }
    
    public string NamePl { get; private set; }
    
    public ICollection<Job>? Jobs { get; set; }
}