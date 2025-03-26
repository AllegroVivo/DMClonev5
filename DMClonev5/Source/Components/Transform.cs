using System;
using Microsoft.Xna.Framework;

namespace DungeonMaker.Components;

public sealed class Transform : IComponent
{
    public Vector2 Position { get; set; }
}