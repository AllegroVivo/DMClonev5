using System;
using System.Collections.Generic;
using DungeonMaker.Components;
using DungeonMaker.Core;
using DungeonMaker.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Dungeon;

public class DungeonTile : IComponent
{
    public Point GridPosition { get; init; }
    public DMTileType Type { get; set; }
    public Entity? DeployedRoom { get; set; }
    public List<Entity> DeployedUnits { get; } = [];

    public RenderTarget2D RenderTarget { get; } = new(GameContext.GraphicsDevice, GameContext.TileSize, GameContext.TileSize);
    
    public static Texture2D? EntranceTexture { get; set; }
    public static Texture2D? EmptyTexture { get; set; }
    
    public Int32 DisplaySize => Type == DMTileType.Boss
        ? (Int32)(GameContext.TileSize * 1.25f)
        : GameContext.TileSize;
    
    public void Draw()
    {
        GameContext.GraphicsDevice.SetRenderTarget(RenderTarget);
        GameContext.GraphicsDevice.Clear(Color.Transparent);
        
        SpriteBatch sb = GameContext.SecondarySpriteBatch;
        sb.Begin();
        
        if (Type == DMTileType.RoomSlot)
        {
            if (DeployedRoom != null)
            {
                SpriteComponent sprite = GameContext.EntityManager.GetComponent<SpriteComponent>(DeployedRoom);
                sb.Draw(sprite.Texture, Vector2.Zero, Color.White);
            }
            else
            {
                if (EmptyTexture == null)
                {
                    Texture2D[] frames = TextureManager.GetAnimation(DMObjectType.Room, "Empty", DMAnimationType.Static);
                    Int32 rand = GameContext.Random.Next(0, frames.Length); 
                    EmptyTexture = frames[rand];
                }
                sb.Draw(EmptyTexture, Vector2.Zero, Color.White);
            }
            
            for (Int32 i = 0; i < DeployedUnits.Count; i++)
            {
                var unit = DeployedUnits[i];
                var sprite = GameContext.EntityManager.GetComponent<SpriteComponent>(unit);
                Vector2 offset = new(i * 10 - 150, GameContext.TileSize - sprite.Texture.Height);
                sb.Draw(sprite.Texture, offset, Color.White);
            }
        }
        else if (Type == DMTileType.Entrance)
        {
            EntranceTexture ??= TextureManager.GetAnimation(DMObjectType.Room, "Entrance", DMAnimationType.Static)[0];
            sb.Draw(EntranceTexture, Vector2.Zero, Color.White);
        }
        
        sb.End();
        GameContext.GraphicsDevice.SetRenderTarget(null);
    }
}