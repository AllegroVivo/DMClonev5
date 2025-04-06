using System;
using DungeonMaker.Entities;

namespace DungeonMaker.Events;

public sealed class HeroEnteredRoomEvent(Entity hero, Entity room) : IGameEvent
{
    public Entity TriggerHero { get; } = hero;
    public Entity Room { get; } = room;
}