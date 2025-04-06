using System;

namespace DungeonMaker.Objects;

public sealed class DMHero(Int32 id, String name, Int32 rank) : DMUnit(id, name, rank, DMObjectType.Hero)
{
    public HeroStatData Stats { get; } = new();
}