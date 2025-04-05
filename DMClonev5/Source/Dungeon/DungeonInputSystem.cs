using System;
using System.Diagnostics;
using DungeonMaker.Core;
using DungeonMaker.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonMaker.Dungeon;

public class DungeonInputSystem
{
    public void Update()
    {
        MouseState mouse = Mouse.GetState();
        Vector2 mousePosition = new(mouse.X, mouse.Y);

        foreach (DungeonTile tile in GameContext.Dungeon.Tiles)
        {
            if (tile.Type != DMTileType.RoomSlot)
                continue;
            
            Vector2 tilePos = GetTileScreenPosition(tile);
            var bounds = new Rectangle((Int32)tilePos.X, (Int32)tilePos.Y, GameContext.TileSize, GameContext.TileSize);

            if (bounds.Contains(mousePosition))
            {
                if (GameContext.InputManager.IsLeftClick() && tile.DeployedRoom == null)
                    DeployRoom(tile);
                else if (GameContext.InputManager.IsRightClick() && tile.DeployedRoom != null)
                    DeployUnit(tile);
            }
        }
    }
    
    private void DeployRoom(DungeonTile tile)
    {
        // For now, just deploy a test room
        var go = ObjectSpawner.Spawn(DMObjectType.Room, "Arena");
        tile.DeployedRoom = go.Entity;
    }
    
    public void DeployUnit(DungeonTile tile)
    {
        var go = ObjectSpawner.Spawn(DMObjectType.Monster, "Bat");
        tile.DeployedUnits.Add(go.Entity);
        
        Debug.Assert(tile.DeployedRoom != null, "Tile has no deployed room");
        EventBus.Publish(new MonsterDeployedEvent(go.Entity, tile.DeployedRoom));
    }
    
    private Vector2 GetTileScreenPosition(DungeonTile tile)
    {
        Int32 x = tile.GridPosition.X;
        Int32 y = tile.GridPosition.Y;

        if (tile.Type == DMTileType.Boss)
        {
            return new Vector2(
                20,
                GameContext.DungeonPaddingY + y * (GameContext.TileSize + GameContext.TilePadding) - (GameContext.TileSize * 0.125f) - 50
            );
        }
        
        return new Vector2(
            GameContext.DungeonPaddingX + GetColumnOffset(x),
            GameContext.DungeonPaddingY + y * (GameContext.TileSize + GameContext.TilePadding)
        );
    }
    
    private Int32 GetColumnOffset(Int32 column)
    {
        if (column == 0)
            return 0;

        return (Int32)((column - 1) * (GameContext.TileSize + GameContext.TilePadding) + (GameContext.TileSize * 1.25f) + GameContext.TilePadding);
    }
}