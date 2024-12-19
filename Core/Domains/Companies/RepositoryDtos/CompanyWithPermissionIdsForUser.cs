namespace Core.Domains.Companies.RepositoryDtos;

public record CompanyWithPermissionIdsForUser(Company? Company, ICollection<long> PermissionIds);