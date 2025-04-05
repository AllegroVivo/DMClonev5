using System;
using System.Collections.Generic;
using DungeonMaker.Core;

namespace DungeonMaker.Objects;

public abstract class DMUnit(Int32 id, String name, Int32 rank, DMObjectType type) 
    : DMObject(id, name, String.Empty, rank, type)
{
    public StatData Stats { get; } = new();
    public List<SpriteMetadata> Sprites { get; } = [];
}