using System;

namespace DungeonMaker.Components;

public sealed class Level : IComponent
{
    private const Int32 MaxUpgrades = 10;
    
    public Int32 Current { get; private set; } = 1;
    public Int32 Experience { get; private set; } = 0;
    public Int32 Upgrades { get; private set; } = 0;
}