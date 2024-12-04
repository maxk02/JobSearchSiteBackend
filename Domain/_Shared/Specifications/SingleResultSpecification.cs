using System.Linq.Expressions;

namespace Domain._Shared.Specifications;

public record OrderBySpecification<T>(Expression<Func<T, object>> Expression, bool Ascending = true);

public class SingleResultSpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; set; }
    public List<Expression<Func<T, object>>> Includes { get; set; } = [];
    public List<OrderBySpecification<T>> OrderBy { get; set; } = [];
}