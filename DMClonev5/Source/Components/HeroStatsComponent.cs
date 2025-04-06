using System;
using System.Collections.Generic;

namespace DungeonMaker.Components;

public sealed class HeroStatsComponent : IComponent
{
    private readonly Dictionary<DMStatType, HeroStatComponent> _stats = new();
    
    public HeroLifeComponent Life => (HeroLifeComponent)_stats[DMStatType.LIFE];
    public HeroStatComponent Attack => _stats[DMStatType.ATK];
    public HeroStatComponent Defense => _stats[DMStatType.DEF];
}

public class HeroStatComponent(Single baseValue, Single growth, DMStatType type) : DMStatComponent(baseValue, type)
{
    private readonly Single _growth = growth;

    public void SetLevel(Int32 level) => BaseValue = GetStatAtLevel(level);
    private Single GetStatAtLevel(Int32 level) => BaseValue + (_growth * (level - 1));
}

public class HeroLifeComponent : HeroStatComponent
{
    public Single Current { get; private set; }
    
    public HeroLifeComponent(Single baseValue, Single growth)
        : base(baseValue, growth, DMStatType.LIFE)
    {
        SetLevel(1);
        SetToFull();
    }
    
    public void Damage(Single amount) => Current = MathF.Max(Current - amount, 0);
    public void Heal(Single amount) => Current = MathF.Min(Current + amount, CurrentValue);
    
    public void SetToFull() => Current = CurrentValue;
    public Single CurrentValue => Current = Calculate();
}