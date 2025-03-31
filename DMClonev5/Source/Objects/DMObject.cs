using System;
using DungeonMaker.Entities;

namespace DungeonMaker.Objects;

public abstract class DMObject(Int32 id, String name, String description, Int32 rank, DMObjectType type)
{
    public Int32 Id { get; } = id;
    public String Name { get; } = name;
    public String Description { get; } = description;
    public Int32 Rank { get; } = rank;
    public DMObjectType Type { get; } = type;
}