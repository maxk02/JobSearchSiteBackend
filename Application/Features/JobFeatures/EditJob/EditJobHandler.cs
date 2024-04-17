// using Application.Providers;
// using Application.Repositories;
// using Application.Repositories.Common;
// using AutoMapper;
// using Domain.Entities;
// using MediatR;
//
// namespace Application.Features.JobFeatures.EditJob;
//
// public sealed class EditJobHandler : IRequestHandler<EditJobRequest>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IJobRepository _jobRepository;
//     private readonly IAddressRepository _addressRepository;
//     private readonly ICompanyRepository _companyRepository;
//     private readonly IMapper _mapper;
//     private readonly ICurrentUserProvider _currentUserProvider;
//     
//     public EditJobHandler(IUnitOfWork unitOfWork, IJobRepository jobRepository,
//         IAddressRepository addressRepository, ICompanyRepository companyRepository,
//         IMapper mapper, ICurrentUserProvider currentUserProvider)
//     {
//         _unitOfWork = unitOfWork;
//         _jobRepository = jobRepository;
//         _addressRepository = addressRepository;
//         _companyRepository = companyRepository;
//         _mapper = mapper;
//         _currentUserProvider = currentUserProvider;
//     }
//
//     public async Task Handle(EditJobRequest request, CancellationToken cancellationToken)
//     {
//         var currentUserId = _currentUserProvider.UserId;
//         if (currentUserId is null)
//             return;
//
//         if (request.Id is null)
//             return;
//
//         var job = await _jobRepository.GetManageModeJob((int)request.Id, (int)currentUserId, cancellationToken);
//         
//         if (job.Tags.SelectMany(x => x.UserTagPermissionSets).Where(x => x.UserId == currentUserId) is null)
//             return;
//         if (companyPermissionSet.CanCreateJobs == false)
//             return;
//         
//         var newJobId = await _jobRepository.Create(job);
//
//         if (request.Addresses != null)
//         {
//             var addresses = _mapper.Map<List<Address>>(request.Addresses);
//             foreach (var address in addresses)
//             {
//                 _addressRepository.CreateForJob(address, newJobId);
//             }
//         }
//
//         if (request.ContractTypeIds != null)
//         {
//             foreach (var contractTypeId in request.ContractTypeIds)
//             {
//                 if (contractTypeId is null or < 1)
//                     continue;
//                 _jobRepository.AttachContractType(newJobId, (int)contractTypeId);
//             }
//         }
//         
//         _jobRepository.AddUserPermissionSet(new UserJobPermissionSet((int)currentUserId, newJobId, true, true));
//
//         await _unitOfWork.Save(cancellationToken);
//
//         return new CreateJobResponse(newJobId);
//     }
// }