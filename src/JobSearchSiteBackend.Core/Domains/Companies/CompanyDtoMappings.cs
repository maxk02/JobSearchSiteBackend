using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Companies.Dtos;

namespace JobSearchSiteBackend.Core.Domains.Companies;

public class CompanyDtoMappings : Profile
{
    public CompanyDtoMappings()
    {
        CreateMap<Company, CompanyDto>();
    }
}