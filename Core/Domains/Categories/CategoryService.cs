using Core.Domains.Categories.UseCases.GetAllCategories;
using Shared.Result;

namespace Core.Domains.Categories;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<Result<IEnumerable<GetAllCategoriesResponse>>> GetAllCategoriesAsync()
    {
        var categoriesResponse = await categoryRepository.GetAllCategoriesAsResponseDtoAsync();
        
        return categoriesResponse;
    }
}