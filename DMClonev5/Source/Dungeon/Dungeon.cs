using System;
using System.Collections.Generic;
using System.Linq;
using DungeonMaker.Core;
using DungeonMaker.Entities;
using DungeonMaker.Utilities;
using Microsoft.Xna.Framework;

namespace DungeonMaker.Dungeon;

public class DungeonGrid
{
    public const Int32 MaxWidth = 8;
    public const Int32 MaxHeight = 5;
    
    public Int32 ActiveLeft { get; private set; } = 0;
    public Int32 ActiveRight { get; private set; } = 3;
    public Int32 ActiveTop { get; private set; } = 1;
    public Int32 ActiveBottom { get; private set; } = 3;
    
    public Int32 ActiveWidth => ActiveRight - ActiveLeft + 1;
    public Int32 ActiveHeight => ActiveBottom - ActiveTop + 1;
    
    public DungeonTile[,] Tiles { get; } = new DungeonTile[MaxWidth, MaxHeight];
    
    private static EntityManager EntityManager => GameContext.EntityManager;
    
    public Int32 CurrentWidth { get; private set; }
    
    public DungeonGrid()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        const Int32 bossRow = MaxHeight / 2;

        for (Int32 x = 0; x < MaxWidth; x++)
        {
            for (Int32 y = 0; y < MaxHeight; y++)
            {
                DMTileType type = x switch
                {
                    0 => (y == bossRow) ? DMTileType.Boss : DMTileType.Empty,
                    >= 1 when x <= ActiveRight && y >= ActiveTop && y <= ActiveBottom => DMTileType.RoomSlot,
                    _ => DMTileType.Empty
                };

                Tiles[x, y] = new DungeonTile
                {
                    GridPosition = new Point(x, y),
                    Type = type
                };
            }
        }

        UpdateEntrances();
    }

    public void DeployRoom(Point position, String roomName)
    {
        if (position.X < 0 || position.X >= MaxWidth || position.Y < 0 || position.Y >= MaxHeight)
            return;

        var tile = Tiles[position.X, position.Y];
        if (tile.Type != DMTileType.RoomSlot)
            return;

        var room = ObjectSpawner.Spawn(DMObjectType.Room, "Arena");
        tile.DeployedRoom = room.Entity;
        
        Logger.Info($"Deployed room '{roomName}' at {position}");
    }
    
    public void UpdateEntrances()
    {
        // Clear old entrances
        for (Int32 x = 0; x < MaxWidth; x++)
        {
            for (Int32 y = 0; y < MaxHeight; y++)
            {
                if (Tiles[x, y].Type == DMTileType.Entrance)
                    Tiles[x, y].Type = DMTileType.Empty;
            }
        }

        Int32 entranceColumn = ActiveRight + 1;
        if (entranceColumn >= MaxWidth)
            return;

        const Int32 middleRow = MaxHeight / 2;

        // Top entrance → ActiveTop row
        if (ActiveTop is >= 0 and < MaxHeight)
            Tiles[entranceColumn, ActiveTop].Type = DMTileType.Entrance;

        // Middle entrance → Middle row
        if (middleRow >= ActiveTop && middleRow <= ActiveBottom)
            Tiles[entranceColumn, middleRow].Type = DMTileType.Entrance;

        // Bottom entrance → ActiveBottom row
        if (ActiveBottom is >= 0 and < MaxHeight)
            Tiles[entranceColumn, ActiveBottom].Type = DMTileType.Entrance;
    }
    
    public Int32 GetCurrentWidth()
    {
        Int32 maxWidth = 0;

        for (Int32 x = 0; x < MaxWidth; x++)
        {
            for (Int32 y = 0; y < MaxHeight; y++)
            {
                if (Tiles[x, y].Type == DMTileType.RoomSlot)
                {
                    maxWidth = x + 1; // +1 because width is inclusive
                    break; // found a room in this column, move to next
                }
            }
        }

        return maxWidth;
    }

    public void RecalculateWidth()
    {
        CurrentWidth = GetCurrentWidth();
        Logger.Debug($"Current width of the dungeon grid: {CurrentWidth}");
    }
    
    public void ExpandWidth()
    {
        if (ActiveRight + 1 >= MaxWidth) 
            return;

        ActiveRight++;

        for (Int32 y = ActiveTop; y <= ActiveBottom; y++)
            Tiles[ActiveRight, y].Type = DMTileType.RoomSlot;

        UpdateEntrances();
        Logger.Debug($"Expanded width to {ActiveRight}");
    }

    public void ExpandHeight(Boolean expandTop)
    {
        if (expandTop)
        {
            if (ActiveTop <= 0)
                return;
            
            ActiveTop--;
            
            for (Int32 x = 1; x <= ActiveRight; x++)
                Tiles[x, ActiveTop].Type = DMTileType.RoomSlot;
        }
        else
        {
            if (ActiveBottom + 1 >= MaxHeight)
                return;
            
            ActiveBottom++;
            
            for (Int32 x = 1; x <= ActiveRight; x++)
                Tiles[x, ActiveBottom].Type = DMTileType.RoomSlot;
        }

        UpdateEntrances();
        Logger.Debug($"Expanded height to {ActiveTop} - {ActiveBottom}");
    }
}