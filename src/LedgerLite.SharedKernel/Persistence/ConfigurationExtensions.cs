using System.Linq.Expressions;
using Ardalis.SmartEnum;
using LedgerLite.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerLite.SharedKernel.Persistence;

public static class ConfigurationExtensions
{
    public static void IsDomainEntity<T>(this EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.HasKey(keyExpression: x => x.Id);
        builder.Property(propertyExpression: x => x.Id).ValueGeneratedNever();
    }

    public static void HasEnumeration<T, TProp>(
        this EntityTypeBuilder<T> builder,
        Expression<Func<T, TProp?>> propertyExpression)
        where TProp : SmartEnum<TProp>
        where T : class
    {
        builder.Property(propertyExpression: propertyExpression)
            .HasConversion(
                convertToProviderExpression: prop => prop!.Value,
                convertFromProviderExpression: value => SmartEnum<TProp>.FromValue(value));
    }

    public static ModelBuilder ConfigureEnumeration<TEnum>(this ModelBuilder builder) where TEnum : SmartEnum<TEnum>
    {
        var enumBuilder = builder.Entity<TEnum>();

        enumBuilder.HasKey(keyExpression: x => x.Value);
        enumBuilder.Property(propertyExpression: x => x.Name).IsRequired();
        enumBuilder.HasData(data: SmartEnum<TEnum>.List);

        return builder;
    }
}