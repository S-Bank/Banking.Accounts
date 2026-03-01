using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Banking.Accounts.Infrastructure.Storage.Context;

public sealed class IdValueConverter<TId> : ValueConverter<TId, Guid>
 where TId : class
{
    // Компилируем функции один раз при инициализации класса
    private static readonly Func<Guid, TId> _factory = CreateFactory();
    private static readonly Func<TId, Guid> _getter = CreateGetter();

    public IdValueConverter() : base(
        id => _getter(id),
        value => _factory(value))
    { }

    private static Func<Guid, TId> CreateFactory()
    {
        var parameter = Expression.Parameter(typeof(Guid), "value");
        var constructor = typeof(TId).GetConstructor(new[] { typeof(Guid) })
            ?? throw new InvalidOperationException($"{typeof(TId).Name} must have a constructor with a single Guid parameter.");

        var body = Expression.New(constructor, parameter);
        return Expression.Lambda<Func<Guid, TId>>(body, parameter).Compile();
    }

    private static Func<TId, Guid> CreateGetter()
    {
        var parameter = Expression.Parameter(typeof(TId), "id");
        var property = typeof(TId).GetProperty("Value")
            ?? throw new InvalidOperationException($"{typeof(TId).Name} must have a 'Value' property.");

        var body = Expression.Property(parameter, property);
        return Expression.Lambda<Func<TId, Guid>>(body, parameter).Compile();
    }
}
