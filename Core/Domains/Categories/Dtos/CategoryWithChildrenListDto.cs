namespace Core.Domains.Categories.Dtos;

public record CategoryWithChildrenListDto(long Id, string Name, IList<CategoryWithChildrenListDto> Children);