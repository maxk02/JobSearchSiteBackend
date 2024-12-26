using Core.Domains._Shared.UseCaseStructure;
using Shared.Result;

namespace Core.Domains.Categories.UseCases.GetAllCategories;

public record GetAllCategoriesRequest() : IRequest<Result<ICollection<GetAllCategoriesResponse>>>;