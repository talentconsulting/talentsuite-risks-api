﻿using static TalentConsulting.TalentSuite.RisksApi.Endpoints.GetRisksEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Models;

internal record struct SafePageParameters(int? RequestedPage, int? RequestedPageSize)
{
    public readonly int Page => Math.Clamp(RequestedPage ?? 1, 1, 999);
    public readonly int PageSize => Math.Clamp(RequestedPageSize ?? 10, 1, 100);

    public static SafePageParameters From(PagingParametersDto parameters) => new (parameters.Page, parameters.PageSize);
};