using System;

namespace DungeonMaker.Objects;

public abstract class DMUnit(Int32 id, String name, String description, Int32 rank, DMObjectType type) 
    : DMObject(id, name, description, rank, type)
{
    protected override void InitComponents()
    {
        
    }
}