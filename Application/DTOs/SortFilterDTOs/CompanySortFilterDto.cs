using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class CompanySortFilterDto : BaseSortFilterDto
{
    public IList<int> IdList { get; set; } = [];
    public IList<int> CountryIdList { get; set; } = [];
}