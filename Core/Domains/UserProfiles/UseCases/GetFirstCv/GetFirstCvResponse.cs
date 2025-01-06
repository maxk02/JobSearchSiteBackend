using Core.Domains._Shared.Pagination;
using Core.Domains.Cvs.Dtos;
using Core.Domains.PersonalFiles.Dtos;

namespace Core.Domains.UserProfiles.UseCases.GetFirstCv;

public record GetFirstCvResponse(
    CvDto? CvDto);