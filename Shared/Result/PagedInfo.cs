using System.Text.Json.Serialization;

namespace Shared.Result;

public class PagedInfo
{
    public PagedInfo(long currentPage, long pageSize, long totalCount)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages =  (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
        
    [JsonInclude]
    public long CurrentPage { get; }
    [JsonInclude]
    public long PageSize { get; }
    [JsonInclude]
    public long TotalCount { get; }
    [JsonInclude]
    public long TotalPages { get; }
}