using System.Collections.Immutable;
using Core.Domains._Shared.Entities;
using Core.Domains.Cvs;
using Core.Domains.Jobs;

namespace Core.Domains.Categories;

public class Category : EntityBase
{
    public static readonly ImmutableArray<Category> AllValues =
    [
        new Category(1)
    ];
    
    public static readonly ImmutableArray<long> AllIds = [..AllValues.Select(category => category.Id)];
    
    private Category(long id) : base(id) { }
    
    public virtual ICollection<Job>? Jobs { get; set; }
    public virtual ICollection<Cv>? Cvs { get; set; }
}