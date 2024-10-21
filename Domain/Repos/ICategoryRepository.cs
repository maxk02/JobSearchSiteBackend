using Domain.Entities;
using Domain.Entities.Categories;
using Domain.Shared.Repos;

namespace Domain.Repos;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IList<Category>> GetListForParentId(long? parentId);
}