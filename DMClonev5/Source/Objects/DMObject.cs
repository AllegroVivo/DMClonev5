using System;
using DungeonMaker.Entities;

namespace DungeonMaker.Objects;

public abstract class DMObject
{
    public Int32 Id { get; }
    public String Name { get; }
    public String Description { get; }
    public Int32 Rank { get; }
    public DMObjectType Type { get; }
    
    protected DMObject(Int32 id, String name, String description, Int32 rank, DMObjectType type)
    {
        Id = id;
        Name = name;
        Description = description;
        Rank = rank;
        Type = type;
        
        InitComponents();
    }

    protected abstract void InitComponents();
}