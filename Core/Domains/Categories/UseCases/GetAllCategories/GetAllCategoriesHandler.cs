using Core.Domains._Shared.UseCaseStructure;
using Core.Domains.Categories.Dtos;
using Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Core.Domains.Categories.UseCases.GetAllCategories;

public class GetAllCategoriesHandler(MainDataContext context) 
    : IRequestHandler<GetAllCategoriesRequest, Result<GetAllCategoriesResponse>>
{
    public async Task<Result<GetAllCategoriesResponse>> Handle(GetAllCategoriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = 
            from c in context.Categories
            join cc in context.CategoryClosures
                on c.Id equals cc.DescendantId
            where cc.Depth == 1
            select new CategoryWithParentIdDto
            (
                c.Id,
                c.Name,
                cc.AncestorId
            );
        
        var categoryWithParentIdList = await query.ToListAsync(cancellationToken);
        
        var tree = BuildCategoryTree(categoryWithParentIdList);

        return new GetAllCategoriesResponse(tree);
    }
    
    private List<CategoryWithChildrenListDto> BuildCategoryTree(IEnumerable<CategoryWithParentIdDto> categories)
    {
        var lookup = categories.ToLookup(x => x.ParentId);
        return BuildNodes(null);

        List<CategoryWithChildrenListDto> BuildNodes(long? parentId)
        {
            return lookup[parentId]
                .Select(x => new CategoryWithChildrenListDto
                (
                    x.Id,
                    x.Name,
                    BuildNodes(x.Id)
                ))
                .ToList();
        }
    }
}