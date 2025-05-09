using JobSearchSiteBackend.Core.Domains._Shared.Enums;

namespace JobSearchSiteBackend.Core.Domains.Jobs.Dtos;

public record JobSalaryInfoDto(
    decimal? Minimum,
    decimal? Maximum,
    Currency Currency,
    UnitOfTime UnitOfTime,
    bool? IsAfterTaxes);