using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class CompanySortFilterDto : BaseSortFilterDto
{
    public IList<long> IdList { get; set; } = [];
    public IList<long> CountryIdList { get; set; } = [];
}