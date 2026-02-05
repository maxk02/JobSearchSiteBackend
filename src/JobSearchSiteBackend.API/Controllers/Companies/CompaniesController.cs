using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using JobSearchSiteBackend.API.Controllers.Companies.Dtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.AddCompanyEmployeeInvitation;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.DeleteCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalance;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyBalanceTransactions;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyEmployees;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobManagementCardDtos;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyManagementNavbarDto;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetJobApplicationTags;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyEmployee;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.RemoveCompanyLastVisitedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.SearchCompanySharedJobs;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.TopUpCompanyBalance;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UpdateCompany;
using JobSearchSiteBackend.Core.Domains.Companies.UseCases.UploadCompanyAvatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSearchSiteBackend.API.Controllers.Companies;

[ApiController]
[Route("api/companies")]
[Authorize]
public class CompaniesController(IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddCompanyResponse>> AddCompany(
        [FromBody] AddCompanyRequest request,
        [FromServices] AddCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddCompanyCommand(request.Name, request.Description,
            request.CountryId, request.CountrySpecificFieldsJson);
        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<AddCompanyResponse>(x)));
    }
    
    [HttpPost]
    [Route("{companyId:long:min(1)}/management/employees")]
    public async Task<ActionResult<AddCompanyResponse>> AddCompanyEmployee(
        [FromRoute] long companyId,
        [FromBody] AddCompanyEmployeeRequest request,
        [FromServices] AddCompanyEmployeeHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddCompanyEmployeeCommand(companyId, request.UserId);
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("{companyId:long:min(1)}/management/employees/invitations")]
    public async Task<ActionResult> AddCompanyEmployeeInvitation(
        [FromRoute] long companyId,
        [FromBody] AddCompanyEmployeeInvitationRequest request,
        [FromServices] AddCompanyEmployeeInvitationHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new AddCompanyEmployeeInvitationCommand(companyId, request.InvitedUserEmail);
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }

    [HttpDelete("{id:long:min(1)}")]
    public async Task<ActionResult> DeleteCompany(
        [FromRoute] long id,
        [FromServices] DeleteCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCompanyCommand(id);
        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:long:min(1)}")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompanyResponse>> GetCompany(
        [FromRoute] long id,
        [FromServices] GetCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyQuery(id);
        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyResponse>(x)));
    }
    
    [HttpGet("{companyId:long:min(1)}/management/balance")]
    public async Task<ActionResult<GetCompanyBalanceResponse>> GetCompanyBalance(
        [FromRoute] long companyId,
        [FromServices] GetCompanyBalanceHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyBalanceQuery(companyId);
        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyBalanceResponse>(x)));
    }

    [HttpGet("{companyId:long:min(1)}/management/balance/transactions")]
    public async Task<ActionResult<GetCompanyBalanceTransactionsResponse>> GetCompanyBalanceTransactions(
        [FromRoute] long companyId,
        [FromQuery] GetCompanyBalanceTransactionsRequest request,
        [FromServices] GetCompanyBalanceTransactionsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyBalanceTransactionsQuery(companyId, request.Page, request.Size);
        var result = await handler.Handle(query, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyBalanceTransactionsResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management/employees")]
    public async Task<ActionResult<GetCompanyEmployeesResponse>> GetCompanyEmployees(
        [FromRoute] long id,
        [FromQuery] GetCompanyEmployeesRequest request,
        [FromServices] GetCompanyEmployeesHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyEmployeesQuery(id, request.Query, request.Page, request.Size);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyEmployeesResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management/jobs")]
    public async Task<ActionResult<GetCompanyJobManagementCardDtosResponse>> GetCompanyJobManagementCardDtos(
        [FromRoute] long id,
        [FromQuery] GetCompanyJobManagementCardDtosRequest request,
        [FromServices] GetCompanyJobManagementCardDtosHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyJobManagementCardDtosQuery(id, request.Query, request.Page,
            request.Size, request.MustHaveSalaryRecord, request.LocationId, request.EmploymentOptionIds,
            request.CategoryIds, request.ContractTypeIds);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyJobManagementCardDtosResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/jobs")]
    [AllowAnonymous]
    public async Task<ActionResult<GetCompanyJobsResponse>> GetCompanyJobs(
        [FromRoute] long id,
        [FromQuery] GetCompanyJobsRequest request,
        [FromServices] GetCompanyJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyJobsQuery(id, request.Query, request.Page, request.Size,
            request.MustHaveSalaryRecord, request.EmploymentTypeIds, request.CategoryIds, request.ContractTypeIds);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyJobsResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management/last-visited-jobs")]
    public async Task<ActionResult<GetCompanyLastVisitedJobsResponse>> GetCompanyLastVisitedJobs(
        [FromRoute] long id,
        [FromServices] GetCompanyLastVisitedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyLastVisitedJobsQuery(id);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x =>  mapper.Map<GetCompanyLastVisitedJobsResponse>(x)));
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management")]
    public async Task<ActionResult<GetCompanyManagementNavbarDtoResponse>> GetCompanyManagementNavbarDto(
        [FromRoute] long id,
        [FromServices] GetCompanyManagementNavbarDtoHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyManagementNavbarDtoQuery(id);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<GetCompanyManagementNavbarDtoResponse>(x)));
    }

    [HttpGet]
    [Route("{id:long:min(1)}/management/job-application-tags")]
    public async Task<ActionResult<GetJobApplicationTagsResponse>> GetJobApplicationTags(
        [FromRoute] long id,
        [FromQuery] GetJobApplicationTagsRequest request,
        [FromServices] GetJobApplicationTagsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationTagsQuery(id, request.SearchQuery, request.Size);
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<GetJobApplicationTagsResponse>(x)));
    }
    
    [HttpDelete]
    [Route("{id:long:min(1)}/management/last-visited-jobs")]
    public async Task<ActionResult> RemoveCompanyAllLastVisitedJobs(
        [FromRoute] long id,
        [FromServices] RemoveCompanyLastVisitedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCompanyLastVisitedJobsCommand(id);
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }
    
    [HttpDelete]
    [Route("{id:long:min(1)}/management/last-visited-jobs/{jobId:long:min(1)}")]
    public async Task<ActionResult> RemoveCompanyLastVisitedJob(
        [FromRoute] long id,
        [FromRoute] long jobId,
        [FromServices] RemoveCompanyLastVisitedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCompanyLastVisitedJobsCommand(id, jobId);
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }
    
    [HttpDelete]
    [Route("{companyId:long:min(1)}/management/employees/{userId:long:min(1)}")]
    public async Task<ActionResult> RemoveCompanyEmployee(
        [FromRoute] long companyId,
        [FromRoute] long userId,
        [FromServices] RemoveCompanyEmployeeHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCompanyEmployeeCommand(companyId, userId);
        
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }
    
    [HttpGet]
    [Route("{id:long:min(1)}/management/jobs/search")]
    public async Task<ActionResult<SearchCompanySharedJobsResponse>> SearchCompanySharedJobs(
        [FromRoute] long id,
        [FromQuery] SearchCompanySharedJobsRequest request,
        [FromServices] SearchCompanySharedJobsHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new SearchCompanySharedJobsQuery(id, request.Query);
        
        var result = await handler.Handle(query, cancellationToken);
    
        return this.ToActionResult(result.Map(x => mapper.Map<SearchCompanySharedJobsResponse>(x)));
    }

    [HttpPatch("{id:long:min(1)}")]
    public async Task<ActionResult> UpdateCompany(
        [FromRoute] long id,
        [FromBody] UpdateCompanyRequest request,
        [FromServices] UpdateCompanyHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCompanyCommand(id, request.Name, request.Description, request.IsPublic);
        
        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result);
    }
    
    [HttpPut]
    [Route("{id:long:min(1)}/avatar")]
    public async Task<ActionResult<UploadCompanyAvatarResponse>> UploadCompanyAvatar(
        [FromRoute] long id,
        [FromForm] IFormFile file,
        [FromServices] UploadCompanyAvatarHandler handler,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return this.ToActionResult(Result.Invalid());
        }

        await using var stream = file.OpenReadStream();
        
        var extension = Path.GetExtension(file.FileName).TrimStart('.');
        var size = file.Length;
        
        var command = new UploadCompanyAvatarCommand(stream, extension, size, id);
        
        var result = await handler.Handle(command, cancellationToken);

        return this.ToActionResult(result.Map(x => mapper.Map<UploadCompanyAvatarResponse>(x)));
    }
    
    [HttpPost]
    [Route("{companyId:long:min(1)}/management/balance/top-ups")]
    public async Task<ActionResult<AddCompanyResponse>> TopUpCompanyBalance(
        [FromRoute] long companyId,
        [FromBody] TopUpCompanyBalanceRequest request,
        [FromServices] TopUpCompanyBalanceHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new TopUpCompanyBalanceCommand(companyId, request.Amount, request.CurrencyId);
        
        var result = await handler.Handle(command, cancellationToken);
    
        return this.ToActionResult(result);
    }
}