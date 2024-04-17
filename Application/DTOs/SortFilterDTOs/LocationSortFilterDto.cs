using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class LocationSortFilterDto : BaseSortFilterDto
{
    public IList<int> IdList { get; set; } = [];
    public int CountryId { get; set; }
}