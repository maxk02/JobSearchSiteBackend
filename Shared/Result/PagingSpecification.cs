namespace Shared.Result;

public record PagingSpecification
{
    public PagingSpecification(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number cannot be less than 1.");
        if (pageSize < 1)
            throw new ArgumentException("Page size cannot be less than 1.");
        
        Skip = (pageNumber - 1) * pageSize;
        Take = pageSize;
    }
    
    public int Skip { get; }
    public int Take { get; }
}