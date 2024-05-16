using FluentValidation;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Validators;

internal class RiskValidator : AbstractValidator<RiskDto>
{
    public RiskValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.ProjectId).NotEmpty();
        RuleFor(dto => dto.Description).NotEmpty();
        RuleFor(dto => dto.Impact).NotEmpty();
        RuleFor(dto => dto.CreatedByReportId).NotEmpty();
        RuleFor(dto => dto.CreatedByUserId).NotEmpty();
        RuleFor(dto => dto.CreatedWhen).NotEmpty();
    }
}
