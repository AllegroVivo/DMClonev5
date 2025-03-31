using System;
using Microsoft.Xna.Framework;

namespace DungeonMaker.Components;

public sealed class TransformComponent : IComponent
{
    public Vector2 Position { get; set; }
    public Single Scale { get; set; } = 1f;
}