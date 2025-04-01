using AutoMapper;
using Core.Domains.UserProfiles.UseCases.UpdateUserProfile;

namespace API.Controllers.UserProfiles;

public class UserProfilesControllerDtosMapper : Profile
{
    public UserProfilesControllerDtosMapper()
    {
        CreateMap<UpdateUserProfileRequestDto, UpdateUserProfileRequest>()
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