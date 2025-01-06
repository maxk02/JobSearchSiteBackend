namespace Core.Domains._Shared.Pagination;

public record PaginationResponse(int TotalCount, int CurrentPage, int PageSize);