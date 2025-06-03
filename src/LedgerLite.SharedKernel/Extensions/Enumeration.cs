using Ardalis.Result;
using Ardalis.SmartEnum;

namespace LedgerLite.SharedKernel.Extensions;

public static class Enumeration<TEnum> where TEnum : SmartEnum<TEnum>
{
    public static Result<TEnum> FromName(string name)
    {
        return SmartEnum<TEnum>.TryFromName(name: name, ignoreCase: true, out var value)
            ? Result.Success(value: value)
            : Result.Invalid(
                new ValidationError($"'{name}' is not a valid {typeof(TEnum).Name}"));
    }
}