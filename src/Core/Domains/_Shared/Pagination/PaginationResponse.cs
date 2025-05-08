namespace Core.Domains._Shared.Pagination;

public record PaginationResponse
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    
    public PaginationResponse(int currentPage, int pageSize, int totalCount)
    {
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}