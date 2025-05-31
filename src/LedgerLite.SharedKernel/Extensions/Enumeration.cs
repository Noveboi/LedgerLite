using Ardalis.Result;
using Ardalis.SmartEnum;

namespace LedgerLite.SharedKernel.Extensions;

public static class Enumeration<TEnum> where TEnum : SmartEnum<TEnum>
{
    public static Result<TEnum> FromName(string name)
    {
        return SmartEnum<TEnum>.TryFromName(name: name, ignoreCase: true, result: out var value)
            ? Result.Success(value: value)
            : Result.Invalid(
                validationError: new ValidationError(errorMessage: $"'{name}' is not a valid {typeof(TEnum).Name}"));
    }
}