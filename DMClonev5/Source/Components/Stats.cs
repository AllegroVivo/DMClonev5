using System;
using System.Collections.Generic;
using System.Diagnostics;
using DungeonMaker.Objects;

namespace DungeonMaker.Components;

public sealed class StatsComponent : IComponent
{
    private readonly Dictionary<DMStatType, DMStatComponent> _stats = new();
    
    public DMLifeComponent Life => (DMLifeComponent)_stats[DMStatType.LIFE];
    public DMStatComponent Attack => _stats[DMStatType.ATK];
    public DMStatComponent BaseDefense => _stats[DMStatType.DEF];

    public void AddStat(DMStatType type, Single baseValue)
    {
        _stats[type] = type switch
        {
            DMStatType.LIFE => new DMLifeComponent((Int32)baseValue),
            _ => new DMStatComponent(baseValue, type)
        };
    }
    
    public DMStatComponent this[DMStatType type] => _stats[type];
    public IEnumerable<DMStatComponent> All => _stats.Values;

    public static StatsComponent FromRaw(Single life, Single atk, Single def)
    {
        StatsComponent stats = new();
        stats.AddStat(DMStatType.LIFE, life);
        stats.AddStat(DMStatType.ATK, atk);
        stats.AddStat(DMStatType.DEF, def);
        return stats;
    }
}

public class DMStatComponent(Single baseValue, DMStatType type)
{
    public DMStatType StatType { get; } = type;

    public Single BaseValue { get; private set; } = baseValue;
    protected Single _flatAdditional { get; private set; }
    protected Single _scalar { get; private set; } = 1.0f;
    protected List<DMStatModification> _modifications { get; } = [];

    protected Single Calculate()
    {
        Reset();
        
        foreach (DMStatModification modification in _modifications)
        {
            _flatAdditional += modification.FlatValue;
            _scalar += modification.PercentValue;
        }
        
        return (BaseValue * _scalar) + _flatAdditional;
    }
    
    public void QueueModification(DMStatModification modification) => _modifications.Add(modification);
    public void ClearModificationsFrom(Object source) => _modifications.RemoveAll(m => m.Source == source);

    public void RequeueModification(DMStatModification modification)
    {
        ClearModificationsFrom(modification.Source);
        QueueModification(modification);
    }

    public void Reset()
    {
        _flatAdditional = 0f;
        _scalar = 1.0f;
    }
}

public class DMLifeComponent : DMStatComponent
{
    public Single Current { get; private set; }
    
    public DMLifeComponent(Int32 baseValue) : base(baseValue, DMStatType.LIFE)
    {
        SetToFull();
    }

    public void Damage(Single amount)
    {
        Debug.Assert(amount >= 0);
        Current = MathF.Max(Current - amount, 0);
    }

    public void Heal(Single amount)
    {
        Debug.Assert(amount >= 0);
        Current = MathF.Min(Current + amount, Calculate());
    }
    
    public void SetToFull() => Current = Calculate();
}

public enum DMScalingType { Flat, Percentage }

public record DMStatModification(Single Value, Object Source, DMScalingType ScalingType = DMScalingType.Flat)
{
    public Single FlatValue => ScalingType == DMScalingType.Flat ? Value : 0;
    public Single PercentValue => ScalingType == DMScalingType.Percentage ? Value : 0;
}