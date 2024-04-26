using FluentValidation;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PutRiskEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Validators;

internal class UpdateRiskRequestValidator : AbstractValidator<UpdateRiskRequest>
{
    public UpdateRiskRequestValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Description).NotEmpty();
        RuleFor(dto => dto.Impact).NotEmpty();
    }
}
