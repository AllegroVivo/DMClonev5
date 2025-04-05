using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Core;

public class SpriteFrame
{
    public Texture2D Texture { get; set; } = null!;
    public String Name { get; set; } = String.Empty;
    public Int32 Order { get; set; } = 0;
}