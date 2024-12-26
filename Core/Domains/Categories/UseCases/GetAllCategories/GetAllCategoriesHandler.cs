using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Categories.UseCases.GetAllCategories;

public class GetAllCategoriesHandler(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetAllCategoriesRequest, Result<ICollection<GetAllCategoriesResponse>>>
{
    public async Task<Result<ICollection<GetAllCategoriesResponse>>> Handle(GetAllCategoriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var categories = await categoryRepository.GetAllAsync();

        return categories.Select(x => new GetAllCategoriesResponse(x.Id, x.Name, x.ParentId)).ToList();
    }
}