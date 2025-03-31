using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Components;

public class SpriteComponent : IComponent
{
    public Texture2D Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }
    public Color Color { get; set; } = Color.White;
}