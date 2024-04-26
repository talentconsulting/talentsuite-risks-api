using FluentValidation;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PutRiskEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Validators;

internal class UpdateRiskRequestValidator : AbstractValidator<UpdateRiskRequest>
{
    public UpdateRiskRequestValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(Risk.MaxDescriptionLength);
        RuleFor(dto => dto.Impact).NotEmpty().MaximumLength(Risk.MaxImpactLength);
    }
}
