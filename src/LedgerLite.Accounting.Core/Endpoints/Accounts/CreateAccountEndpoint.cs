using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Application.Chart;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
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
        _log.Information(messageTemplate: "Account {action}: {number} - {name}", propertyValue0: "CREATE",
            propertyValue1: req.Number, propertyValue2: req.Name);

        var request = await MapToEntity(r: req,
            () => chartService.GetByUserIdAsync(userId: req.UserId, token: ct));
        if (!request.IsSuccess)
        {
            await SendResultAsync(request.ToMinimalApiResult());
            return;
        }

        var creationResult = await accountService.CreateAsync(request: request, token: ct);
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
        var typeConversion = Enumeration<AccountType>.FromName(name: r.Type);
        if (!typeConversion.IsSuccess)
            return typeConversion.Map();

        var currencyConversion = Enumeration<Currency>.FromName(name: r.Currency);
        if (!currencyConversion.IsSuccess)
            return currencyConversion.Map();

        var chart = await getChart();
        if (!chart.IsSuccess)
            return chart.Map();

        var expenseType = ExpenseType.Undefined;
        if (!string.IsNullOrWhiteSpace(r.ExpenseType) && !Enum.TryParse(r.ExpenseType, ignoreCase: true, out expenseType))
        {
            return Result.Invalid(new ValidationError($"{r.ExpenseType} is not a valid expense type."));
        }
        
        return new CreateAccountRequest(
            Name: r.Name,
            Number: r.Number,
            Type: typeConversion.Value,
            Currency: currencyConversion.Value,
            IsPlaceholder: r.IsPlaceholder,
            r.Description ?? "",
            Chart: chart,
            ParentId: r.ParentId,
            Metadata: new AccountMetadata(expenseType));
    }
}

internal sealed record CreateAccountRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
    string Name,
    string Number,
    string Type,
    string Currency,
    bool IsPlaceholder,
    string? Description,
    Guid? ParentId,
    string? ExpenseType);