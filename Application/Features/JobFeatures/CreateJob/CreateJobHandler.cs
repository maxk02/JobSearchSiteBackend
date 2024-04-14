using Application.Providers;
using Application.Repositories;
using Application.Repositories.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.JobFeatures.CreateJob;

public sealed class CreateJobHandler : IRequestHandler<CreateJobRequest, CreateJobResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJobRepository _jobRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;
    
    public CreateJobHandler(IUnitOfWork unitOfWork, IJobRepository jobRepository,
        IAddressRepository addressRepository, ICompanyRepository companyRepository,
        IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _jobRepository = jobRepository;
        _addressRepository = addressRepository;
        _companyRepository = companyRepository;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<CreateJobResponse> Handle(CreateJobRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserProvider.UserId;
        if (currentUserId is null)
            return new CreateJobResponse(null);
        
        var job = _mapper.Map<Job>(request);
        
        var companyPermissionSet = await _companyRepository
            .GetUserPermissionSet(job.CompanyId, (long)currentUserId, cancellationToken);
        if (companyPermissionSet is null)
            return new CreateJobResponse(null);
        if (companyPermissionSet.CanCreateJobs == false)
            return new CreateJobResponse(null);
        
        var newJobId = await _jobRepository.Create(job);

        if (request.Addresses != null)
        {
            var addresses = _mapper.Map<List<Address>>(request.Addresses);
            foreach (var address in addresses)
            {
                _addressRepository.CreateForJob(address, newJobId);
            }
        }

        if (request.ContractTypeIds != null)
        {
            foreach (var contractTypeId in request.ContractTypeIds)
            {
                if (contractTypeId is null or < 1)
                    continue;
                _jobRepository.AttachContractType(newJobId, (long)contractTypeId);
            }
        }
        
        _jobRepository.AddUserPermissionSet(new UserJobPermissionSet((long)currentUserId, newJobId, true, true));

        await _unitOfWork.Save(cancellationToken);

        return new CreateJobResponse(newJobId);
    }
}