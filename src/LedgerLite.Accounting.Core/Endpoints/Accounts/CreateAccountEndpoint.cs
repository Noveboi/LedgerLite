using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;
using LedgerLite.SharedKernel.Extensions;
using Serilog;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed class CreateAccountEndpoint(IAccountService accountService) : Endpoint<CreateAccountRequestDto>
{
    private readonly ILogger _log = Log.ForContext<CreateAccountEndpoint>();

    public override void Configure()
    {        
        Post("");
        Group<AccountEndpointsGroup>();
    }

    public override async Task HandleAsync(CreateAccountRequestDto req, CancellationToken ct)
    {
        _log.Information("Account {action}: {number} - {name}", "CREATE", req.Number, req.Name);
        
        var request = MapToEntity(req);
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
            routeValues: new { account.Id },
            responseBody: AccountResponseDto.FromEntity(account),
            cancellation: ct);
    }

    private static Result<CreateAccountRequest> MapToEntity(CreateAccountRequestDto r)
    {
        var typeConversion = Enumeration<AccountType>.FromName(r.Type);
        var currencyConversion = Enumeration<Currency>.FromName(r.Currency);

        if (!typeConversion.IsSuccess)
            return typeConversion.Map();

        if (!currencyConversion.IsSuccess)
            return currencyConversion.Map();

        return new CreateAccountRequest(
            Name: r.Name,
            Number: r.Number,
            Type: typeConversion.Value,
            Currency: currencyConversion.Value,
            IsPlaceholder: r.IsPlaceholder,
            Description: r.Description ?? "",
            ChartOfAccountsId: r.ChartOfAccountsId,
            ParentId: r.ParentId);
    }
}

internal sealed record CreateAccountRequestDto(
    string Name,
    string Number,
    string Type,
    string Currency,
    bool IsPlaceholder,
    string? Description,
    Guid ChartOfAccountsId,
    Guid? ParentId);