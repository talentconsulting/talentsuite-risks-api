using FluentValidation;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Validators;

internal class CreateRiskRequestValidator : AbstractValidator<CreateRiskRequest>
{
    public CreateRiskRequestValidator()
    {
        RuleFor(dto => dto.ProjectId).NotEmpty();
        RuleFor(dto => dto.Description).NotEmpty();
        RuleFor(dto => dto.Impact).NotEmpty();
        RuleFor(dto => dto.CreatedByReportId).NotEmpty();
        RuleFor(dto => dto.CreatedByUserId).NotEmpty();
    }
}
