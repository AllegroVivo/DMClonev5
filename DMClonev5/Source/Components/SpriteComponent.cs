using System;
using DungeonMaker.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Components;

public class SpriteComponent : IComponent
{
    public Texture2D Texture { get; set; } = null!;
    public Color Color { get; set; } = Color.White;
    public Single Scale { get; set; } = 1f;
}