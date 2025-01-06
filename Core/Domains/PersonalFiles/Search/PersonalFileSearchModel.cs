using Core.Domains._Shared.Search;
using Core.Domains._Shared.ValueEntities;
using Core.Domains.Cvs.ValueEntities;

namespace Core.Domains.PersonalFiles.Search;

public record PersonalFileSearchModel(
    long Id, long UserId,
    string TextContent,
    ICollection<long> JobIdsApplied,
    ICollection<long> JobIdsUnapplied
) : ISearchModel;