﻿using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;

namespace JobSearchSiteBackend.Core.Domains.Companies.UseCases.GetCompanyLastVisitedFolders;

public record GetCompanyLastVisitedFoldersRequest(): IRequest<GetCompanyLastVisitedFoldersResponse>;