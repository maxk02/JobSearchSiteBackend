using Core.Domains.Categories.UseCases.GetAllCategories;
using Shared.Result;

namespace Core.Domains.Categories;

public interface ICategoryService
{
    public Task<Result<IEnumerable<GetAllCategoriesResponse>>> GetAllCategoriesAsync();
}