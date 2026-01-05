using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;

namespace JobSearchSiteBackend.API.Controllers.Companies;

public class CompaniesControllerDtosMapper : Profile
{
    public CompaniesControllerDtosMapper()
    {
        CreateMap<AddCompanyResult, AddCompanyResponse>();

        CreateMap<GetCompanyBalanceResult, GetCompanyBalanceResponse>();
        
        CreateMap<GetCompanyEmployeesResult, GetCompanyEmployeesResponse>();
        
        CreateMap<GetCompanyJobsResult, GetCompanyJobsResponse>();
        
        CreateMap<GetCompanyLastVisitedJobsResult, GetCompanyLastVisitedJobsResponse>();
        
        CreateMap<GetCompanyManagementNavbarDtoResult, GetCompanyManagementNavbarDtoResponse>();
        
        CreateMap<GetCompanyResult, GetCompanyResponse>();
        
        CreateMap<SearchCompanySharedJobsResult, SearchCompanySharedJobsResponse>();

        CreateMap<UploadCompanyAvatarResult, UploadCompanyAvatarResponse>();
    }
}