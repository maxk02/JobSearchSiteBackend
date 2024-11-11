using Application.DTOs.SortFilterDTOs.Common;
using Application.DTOs.SortFilterDTOs.Enums;
using Domain.Shared.ValueEntities;

namespace Application.DTOs.SortFilterDTOs;

public class JobSortFilterDto : BaseSortFilterDto
{
    public JobSortValue SortValue { get; set; }
    
    public IList<int> IdList { get; set; } = [];
    public int CompanyId { get; set; }
    public IList<int> CategoryIdList { get; set; } = [];
    public IList<int> JobContractTypeIdList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    public bool MustHaveSalarySpecified { get; set; } = false;
}