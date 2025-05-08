using Core.Domains._Shared.Enums;

namespace Core.Domains.Jobs.Dtos;

public record JobSalaryInfoDto(
    decimal? Minimum,
    decimal? Maximum,
    Currency Currency,
    UnitOfTime UnitOfTime,
    bool? IsAfterTaxes);