using Application.Repositories.Common;
using Domain.Entities;

namespace Application.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IList<Category>> GetListForParentId(long? parentId);
}