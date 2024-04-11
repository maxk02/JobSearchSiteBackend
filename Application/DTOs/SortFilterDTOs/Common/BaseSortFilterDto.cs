namespace Application.DTOs.SortFilterDTOs.Common;

public class BaseSortFilterDto
{
    public int PageSize { get; set; }
    public bool IsDesc { get; set; } = false;
    public bool IsOffset { get; set; } = false;
}