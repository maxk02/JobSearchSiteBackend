﻿using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.CompanyClaims.UseCases.GetCompanyClaimIdsForUser;

public record GetCompanyClaimIdsForUserRequest(long UserId, long CompanyId) : IRequest<Result<ICollection<long>>>;