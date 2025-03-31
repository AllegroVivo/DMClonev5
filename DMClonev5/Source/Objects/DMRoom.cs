using System;
using System.Collections.Generic;

namespace DungeonMaker.Objects;

public sealed class DMRoom(Int32 id, String name, String description, Int32 rank, DMRoomType type) 
    : DMObject(id, name, description, rank, DMObjectType.Room)
{
    public DMRoomType RoomType { get; } = type;
    public List<EffectData> Effects { get; } = [];
}
