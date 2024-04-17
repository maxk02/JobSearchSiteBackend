using Application.DTOs.SortFilterDTOs.Common;
using Application.DTOs.SortFilterDTOs.Enums;

namespace Application.DTOs.SortFilterDTOs;

public class ApplicationSortFilterDto : BaseSortFilterDto
{
    public ApplicationSortValue SortValue { get; set; }
    
    public int JobId { get; set; }
}