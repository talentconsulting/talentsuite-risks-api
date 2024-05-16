using FluentValidation;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Validators;

internal class CreateRiskRequestValidator : AbstractValidator<CreateRiskRequest>
{
    public CreateRiskRequestValidator()
    {
        RuleFor(dto => dto.ProjectId).NotEmpty();
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(Risk.MaxDescriptionLength);
        RuleFor(dto => dto.Impact).NotEmpty().MaximumLength(Risk.MaxImpactLength);
        RuleFor(dto => dto.CreatedByReportId).NotEmpty();
        RuleFor(dto => dto.CreatedByUserId).NotEmpty();
    }
}
