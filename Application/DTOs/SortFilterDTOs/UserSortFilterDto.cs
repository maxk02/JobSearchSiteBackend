using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class UserSortFilterDto : BaseSortFilterDto
{
    public IList<long> IdList { get; set; } = [];
    public IList<long> LocationIdList { get; set; } = [];
    public IList<long> CategoryIdList { get; set; } = [];
}