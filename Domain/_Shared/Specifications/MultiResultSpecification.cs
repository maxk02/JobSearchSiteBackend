namespace Domain._Shared.Specifications;

public class MultiResultSpecification<T> : SingleResultSpecification<T>
{
    public int? Skip { get; private set; }
    public int? Take { get; private set; }
}