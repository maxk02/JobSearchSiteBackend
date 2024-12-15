namespace Core.Domains._Shared.ValueEntities;

public record SalaryRecord(decimal? Minimum, decimal? Maximum, string CurrencyCode, string UnitOfTime);