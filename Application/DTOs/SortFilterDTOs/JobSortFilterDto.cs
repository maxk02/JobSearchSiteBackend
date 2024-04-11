using Application.DTOs.SortFilterDTOs.Common;
using Application.DTOs.SortFilterDTOs.Enums;
using Domain.JSONEntities;

namespace Application.DTOs.SortFilterDTOs;

public class JobSortFilterDto : BaseSortFilterDto
{
    public JobSortValue SortValue { get; set; }
    
    public IList<long> IdList { get; set; } = [];
    public long CompanyId { get; set; }
    public IList<long> CategoryIdList { get; set; } = [];
    public IList<long> JobContractTypeIdList { get; set; } = [];
    public EmploymentTypeRecord? EmploymentTypeRecord { get; set; }
    public bool MustHaveSalarySpecified { get; set; } = false;
}