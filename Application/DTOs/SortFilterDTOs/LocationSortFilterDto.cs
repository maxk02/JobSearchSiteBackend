using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class LocationSortFilterDto : BaseSortFilterDto
{
    public IList<long> IdList { get; set; } = [];
    public long CountryId { get; set; }
}