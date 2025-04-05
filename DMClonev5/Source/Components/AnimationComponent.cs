using System;
using DungeonMaker.Core;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Components;

public class AnimationComponent : IComponent
{
    public SpriteFrame[] Frames { get; set; } = [];
    public Int32 CurrentFrame { get; set; }
    public Single FrameTime { get; set; } = 0.12f;
    public Single TimeElapsed { get; set; }
    public Boolean Loop { get; set; } = true;
    
    public Texture2D CurrentTexture => Frames.Length > 0 ? Frames[CurrentFrame].Texture : null!;
}