namespace Core.Domains.Categories.UseCases.GetAllCategories;

public record GetAllCategoriesResponse(long Id, string Name, long? ParentId);