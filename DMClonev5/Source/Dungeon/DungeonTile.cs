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

    public static SpriteFrame? EntraceFrame { get; set; }
    public static SpriteFrame? EmptyFrame { get; set; }

    public Int32 DisplaySize => Type == DMTileType.Boss
        ? (Int32)(GameContext.TileSize * 1.25f)
        : GameContext.TileSize;
    
    private static Texture2D GetCurrentFrame(Entity entity)
    {
        var em = GameContext.EntityManager;

        if (em.TryGetComponent<AnimationComponent>(entity, out var anim))
            return anim.CurrentTexture;

        if (em.TryGetComponent<SpriteComponent>(entity, out var sprite))
            return sprite.Texture;

        throw new Exception("Entity has no drawable component");
    }

    public void Draw()
    {
        GameContext.GraphicsDevice.SetRenderTarget(RenderTarget);
        GameContext.GraphicsDevice.Clear(Color.Transparent);

        SpriteBatch sb = GameContext.SecondarySpriteBatch;
        sb.Begin();

        // === Draw Room ===
        if (Type == DMTileType.RoomSlot)
        {
            if (DeployedRoom != null)
            {
                var texture = GetCurrentFrame(DeployedRoom);
                sb.Draw(texture, Vector2.Zero, Color.White);
            }
            else
            {
                if (EmptyFrame == null)
                {
                    SpriteFrame[] frames = TextureManager.GetAnimation(DMObjectType.Room, "Empty", DMAnimationType.Static);
                    Int32 rand = GameContext.Random.Next(0, frames.Length);
                    EmptyFrame = frames[rand];
                }

                sb.Draw(EmptyFrame.Texture, Vector2.Zero, Color.White);
            }
        }
        else if (Type == DMTileType.Entrance)
        {
            EntraceFrame ??= TextureManager.GetAnimation(DMObjectType.Room, "Entrance", DMAnimationType.Static)[0];
            sb.Draw(EntraceFrame.Texture, Vector2.Zero, Color.White);
        }

        DrawUnits(sb);

        sb.End();
        GameContext.GraphicsDevice.SetRenderTarget(null);
    }
    
    public void Draw(SpriteBatch sb, Vector2 screenPosition)
    {
        // Draw room
        if (Type == DMTileType.RoomSlot)
        {
            if (DeployedRoom != null)
            {
                var texture = GetCurrentFrame(DeployedRoom);
                sb.Draw(texture, screenPosition, Color.White);
            }
            else
            {
                if (EmptyFrame == null)
                {
                    SpriteFrame[] frames = TextureManager.GetAnimation(DMObjectType.Room, "Empty", DMAnimationType.Static);
                    Int32 rand = GameContext.Random.Next(0, frames.Length);
                    EmptyFrame = frames[rand];
                }

                sb.Draw(EmptyFrame.Texture, screenPosition, Color.White);
            }
        }
        else if (Type == DMTileType.Entrance)
        {
            EntraceFrame ??= TextureManager.GetAnimation(DMObjectType.Room, "Entrance", DMAnimationType.Static)[0];
            sb.Draw(EntraceFrame.Texture, screenPosition, Color.White);
        }

        // Draw units stacked vertically
        DrawUnits(sb, screenPosition);
    }
    
    private void DrawUnits(SpriteBatch sb)
    {
        if (DeployedUnits.Count == 0)
            return;

        Single padding = 4f;
        Single totalHeight = 0f;

        // Calculate total stack height
        foreach (var unit in DeployedUnits)
        {
            var sprite = GameContext.EntityManager.GetComponent<SpriteComponent>(unit);
            totalHeight += sprite.Texture.Height;
        }
        totalHeight += (DeployedUnits.Count - 1) * padding;

        // Center stack vertically in the RenderTarget
        Single startY = (RenderTarget.Height - totalHeight) / 2f;
        Single currentY = startY;

        foreach (var unit in DeployedUnits)
        {
            var sprite = GameContext.EntityManager.GetComponent<SpriteComponent>(unit);
            Vector2 position = new(
                (GameContext.TileSize - sprite.Texture.Width) / 2f,
                currentY
            );

            sb.Draw(sprite.Texture, position, sprite.Color);
            currentY += sprite.Texture.Height + padding;
        }
    }

    private void DrawUnits(SpriteBatch sb, Vector2 basePosition)
    {
        if (DeployedUnits.Count == 0)
            return;

        Single sliceHeight = GameContext.TileSize / (Single)DeployedUnits.Count;

        for (Int32 i = 0; i < DeployedUnits.Count; i++)
        {
            var unit = DeployedUnits[i];
            var texture = GetCurrentFrame(unit);

            Vector2 position = new(
                basePosition.X + (GameContext.TileSize - texture.Width) / 2f - 40f,
                basePosition.Y + (sliceHeight * i) + (sliceHeight - texture.Height) / 2f - 10f
            );

            sb.Draw(
                texture,
                position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.FlipHorizontally,
                0f
            );
        }
    }
}
