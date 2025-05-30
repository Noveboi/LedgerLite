﻿using LedgerLite.SharedKernel.Domain.Events;

namespace LedgerLite.SharedKernel.Domain;

public abstract class Entity : IHaveDomainEvents
{
    protected Entity() => Id = GenerateId();

    public Guid Id { get; private init; }

    private List<DomainEvent>? _domainEvents;
    IReadOnlyList<DomainEvent> IHaveDomainEvents.DomainEvents => _domainEvents ??= [];
    public void AddDomainEvent<T>(T domainEvent) where T : DomainEvent => (_domainEvents ??= []).Add(domainEvent);
    
    public override bool Equals(object? obj)
    {
        if (obj is Entity entity)
            return Id == entity.Id;

        return false;
    }

    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b) => !(a == b);

    public override int GetHashCode() => Id.GetHashCode();
    private static Guid GenerateId() => Guid.CreateVersion7();
}

internal interface IHaveDomainEvents
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
}