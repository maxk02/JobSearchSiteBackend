using JobSearchSiteBackend.Main.Domains._Shared.Enums;

namespace JobSearchSiteBackend.Main.Domains.Jobs.Dtos;

public record JobSalaryInfoDto(
    decimal? Minimum,
    decimal? Maximum,
    long CurrencyId,
    UnitOfTime UnitOfTime,
    bool? IsAfterTaxes);