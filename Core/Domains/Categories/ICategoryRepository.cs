using Core.Domains.Categories.UseCases.GetAllCategories;
using Shared.Result;

namespace Core.Domains.Categories;

public interface ICategoryRepository
{
    public Task<ICollection<Category>> GetAllAsync();
}