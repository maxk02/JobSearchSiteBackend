namespace Domain.Shared.Specifications;

public class MultiResultSpecificationBase<T> : SingleResultSpecificationBase<T>
{
    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    

    public MultiResultSpecificationBase<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        
        return this;
    }
}