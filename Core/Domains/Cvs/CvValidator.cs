// using Core.Domains._Shared.ValueEntities;
// using Core.Domains.Cvs.ValueEntities;
// using FluentValidation;
// using Shared.FluentValidationAddons;
// using Shared.FluentValidationAddons.StringFiltering;
//
// namespace Core.Domains.Cvs;
//
// public class CvValidator : AbstractValidator<Cv>
// {
//     public CvValidator()
//     {
//         When(x => x.SalaryRecord != null, () =>
//         {
//             RuleFor(x => x.SalaryRecord!).SetValidator(new SalaryRecordValidator());
//         });
//         
//         When(x => x.EmploymentTypeRecord != null, () =>
//         {
//             RuleFor(x => x.EmploymentTypeRecord!).SetValidator(new EmploymentTypeRecordValidator());
//         });
//
//         RuleFor(x => x.EducationRecords)
//             .ForEach(x => x.SetValidator(new EducationRecordValidator()));
//         
//         RuleFor(x => x.WorkRecords)
//             .ForEach(x => x.SetValidator(new WorkRecordValidator()));
//
//         RuleFor(x => x.Skills)
//             .ForEach(x => x.Length(1, 50)
//                 .WhitelistPolicy(new WhitelistPolicy().Letters().Spaces().Digits().Symbols()));
//     }
// }