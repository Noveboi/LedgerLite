using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

internal static class FiscalPeriodToDtoConversions
{
    public static FiscalPeriodDto ToDto(this FiscalPeriod period)
    {
        return FiscalPeriodDto.FromEntity(fiscalPeriod: period);
    }
}