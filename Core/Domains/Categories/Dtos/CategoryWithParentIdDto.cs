namespace Core.Domains.Categories.Dtos;

public record CategoryWithParentIdDto(long Id, string Name, long? ParentId);