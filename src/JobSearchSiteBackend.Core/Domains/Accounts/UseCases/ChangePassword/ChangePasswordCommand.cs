using JobSearchSiteBackend.Core.Domains._Shared.UseCaseStructure;
using Ardalis.Result;

namespace JobSearchSiteBackend.Core.Domains.Accounts.UseCases.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest<Result>;