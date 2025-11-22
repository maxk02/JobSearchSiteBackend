using AutoMapper;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;
using UpdateCompanyRequest = JobSearchSiteBackend.API.Controllers.Companies.Dtos.UpdateCompanyRequest;

namespace JobSearchSiteBackend.API.Controllers.Companies;

public class CompaniesControllerDtosMapper : Profile
{
    public CompaniesControllerDtosMapper()
    {
        CreateMap<UpdateCompanyRequest, UpdateCompanyCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom((_, _, _, context) =>
            {
                if (context.Items.TryGetValue("Id", out var id))
                {
                    return (long)id;
                }

                return 0;
            }));
    }
}