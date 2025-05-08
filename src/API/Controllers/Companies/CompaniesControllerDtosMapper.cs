using AutoMapper;
using Core.Domains.Companies.UseCases.UpdateCompany;

namespace API.Controllers.Companies;

public class CompaniesControllerDtosMapper : Profile
{
    public CompaniesControllerDtosMapper()
    {
        CreateMap<UpdateCompanyRequestDto, UpdateCompanyRequest>()
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