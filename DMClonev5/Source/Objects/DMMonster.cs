using System;

namespace DungeonMaker.Objects;

public sealed class DMMonster(Int32 id, String name, Int32 rank) : DMUnit(id, name, rank, DMObjectType.Monster)
{
    public MonsterStatData Stats { get; } = new();
}