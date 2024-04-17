using Application.DTOs.SortFilterDTOs.Common;

namespace Application.DTOs.SortFilterDTOs;

public class UserSortFilterDto : BaseSortFilterDto
{
    public IList<int> IdList { get; set; } = [];
    public IList<int> LocationIdList { get; set; } = [];
    public IList<int> CategoryIdList { get; set; } = [];
}