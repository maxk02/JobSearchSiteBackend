using AutoMapper;

namespace JobSearchSiteBackend.Core.Domains.Companies.Dtos;

public class CompanyDtoMappings : Profile
{
    public CompanyDtoMappings()
    {
        CreateMap<Company, CompanyDto>();
    }
}