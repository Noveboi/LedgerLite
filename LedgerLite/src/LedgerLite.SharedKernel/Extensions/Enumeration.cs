using Ardalis.Result;
using Ardalis.SmartEnum;

namespace LedgerLite.SharedKernel.Extensions;

public static class Enumeration<TEnum> where TEnum : SmartEnum<TEnum>
{
    public static Result<TEnum> FromName(string name) =>
        SmartEnum<TEnum>.TryFromName(name, ignoreCase: true, out var value)
            ? Result.Success(value)
            : Result.Invalid(new ValidationError($"'{name}' is not a valid {typeof(TEnum).Name}"));
}