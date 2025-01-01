using Core.Domains.Categories.Dtos;

namespace Core.Domains.Categories.UseCases.GetAllCategories;

public record GetAllCategoriesResponse(ICollection<CategoryWithChildrenListDto> CategoriesWithChildrenList);