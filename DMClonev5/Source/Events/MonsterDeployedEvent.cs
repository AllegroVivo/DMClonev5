using System;
using DungeonMaker.Entities;

namespace DungeonMaker.Events;

public sealed class MonsterDeployedEvent(Entity monster, Entity room)
{
    public Entity Monster { get; } = monster;
    public Entity Room { get; } = room;
}