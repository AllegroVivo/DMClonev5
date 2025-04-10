using System;
using Microsoft.Xna.Framework;

namespace DungeonMaker.Components;

public class PositionComponent : IComponent
{
    public Point GridPosition { get; set; }
    public Vector2 WorldPosition { get; set; }
    
    public PositionComponent() { }

    public PositionComponent(Int32 x, Int32 y)
    {
        GridPosition = new Point(x, y);
        WorldPosition = PointToWorld(GridPosition);
    }
    
    public PositionComponent(Point gridPosition, Vector2 worldPosition)
    {
        GridPosition = gridPosition;
        WorldPosition = worldPosition;
    }
    
    public static Vector2 PointToWorld(Point gridPos)
    {
        return new Vector2(
            GameContext.DungeonPaddingX + GetColumnOffset(gridPos.X),
            GameContext.DungeonPaddingY + gridPos.Y * (GameContext.TileSize + GameContext.TilePadding)
        );
    }

    public static Int32 GetColumnOffset(Int32 column)
    {
        return column == 0
            ? 0
            : (Int32)((column - 1) * (GameContext.TileSize + GameContext.TilePadding)
                      + (GameContext.TileSize * 1.25f) + GameContext.TilePadding);
    }
    
    public void SyncWorldPosition() => WorldPosition = PointToWorld(GridPosition);
    
    public void SetGridPosition(Point gridPos)
    {
        GridPosition = gridPos;
        SyncWorldPosition();
    }
}