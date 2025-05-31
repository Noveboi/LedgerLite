using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;
using LedgerLite.SharedKernel.Extensions;
using LedgerLite.SharedKernel.Identity;
using Serilog;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed class CreateAccountEndpoint(IChartOfAccountsService chartService, IAccountService accountService)
    : Endpoint<CreateAccountRequestDto>
{
    private readonly ILogger _log = Log.ForContext<CreateAccountEndpoint>();

    public override void Configure()
    {
        Post("");
        Group<ChartOfAccountsModifyGroup>();
    }

    public override async Task HandleAsync(CreateAccountRequestDto req, CancellationToken ct)
    {
        _log.Information("Account {action}: {number} - {name}", "CREATE", req.Number, req.Name);

        var request = await MapToEntity(req, () => chartService.GetByUserIdAsync(req.UserId, ct));
        if (!request.IsSuccess)
        {
            await SendResultAsync(request.ToMinimalApiResult());
            return;
        }

        var creationResult = await accountService.CreateAsync(request, ct);
        if (!creationResult.IsSuccess)
        {
            await SendResultAsync(creationResult.ToMinimalApiResult());
            return;
        }

        var account = creationResult.Value;

        await SendCreatedAtAsync<GetAccountEndpoint>(
            new { account.Id },
            account.ToDto(),
            cancellation: ct);
    }

    private static async Task<Result<CreateAccountRequest>> MapToEntity(
        CreateAccountRequestDto r,
        Func<Task<Result<ChartOfAccounts>>> getChart)
    {
        var typeConversion = Enumeration<AccountType>.FromName(r.Type);
        if (!typeConversion.IsSuccess)
            return typeConversion.Map();

        var currencyConversion = Enumeration<Currency>.FromName(r.Currency);
        if (!currencyConversion.IsSuccess)
            return currencyConversion.Map();

        var chart = await getChart();
        if (!chart.IsSuccess)
            return chart.Map();

        return new CreateAccountRequest(
            r.Name,
            r.Number,
            typeConversion.Value,
            currencyConversion.Value,
            r.IsPlaceholder,
            r.Description ?? "",
            chart,
            r.ParentId);
    }
}

internal sealed record CreateAccountRequestDto(
    [property: FromClaim(LedgerClaims.UserId)]
    Guid UserId,
    string Name,
    string Number,
    string Type,
    string Currency,
    bool IsPlaceholder,
    string? Description,
    Guid? ParentId);