using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Reporting.Trial;

namespace LedgerLite.Accounting.Reporting.Income.Endpoints;

internal sealed class GetIncomeStatementEndpoint(ReportingUserAuthorization auth, TrialBalanceService service) 
    : Endpoint<IncomeStatementRequest, IncomeStatementDto>
{
    public override void Configure()
    {
        Get("");
        Group<IncomeEndpointGroup>();
    }

    public override async Task HandleAsync(IncomeStatementRequest req, CancellationToken ct)
    {
        var authResult = await auth.AuthorizeAsync(req.UserId, req.PeriodId, ct);
        if (!authResult.IsSuccess)
        {
            await SendResultAsync(authResult.ToMinimalApiResult());
            return;
        }

        var period = authResult.Value;
        var request = new CreateTrialBalanceRequest(period);

        var trialBalance = await service.CreateTrialBalanceAsync(request, ct);
        var incomeStatement = IncomeStatement.Create(trialBalance);

        await SendAsync(IncomeStatementDto.FromEntity(incomeStatement), cancellation: ct);
    }
}