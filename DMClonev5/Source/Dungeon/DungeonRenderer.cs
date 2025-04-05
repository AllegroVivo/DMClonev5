using System;
using System.Collections.Generic;
using DungeonMaker.Components;
 using DungeonMaker.Core;
 using Microsoft.Xna.Framework;
 using Microsoft.Xna.Framework.Graphics;
 
 namespace DungeonMaker.Dungeon;
 
 public class DungeonRenderer
 {
     private static GraphicsDevice GraphicsDevice => GameContext.GraphicsDevice;
     private RenderTarget2D _renderTarget = new(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
     private Boolean _needsRedraw = true;
     private static DungeonGrid Dungeon => GameContext.Dungeon;
     
     public static Texture2D? BossTexture { get; set; }
     private static readonly Texture2D _bgTexture = TextureManager.GetSingleTexture("DungeonBackground");
     private static readonly Texture2D _lineTexture = TextureManager.GetSingleTexture("DungeonConnectingLine");
 
     public void Redraw()
     {
         SpriteBatch sb = GameContext.MainSpriteBatch;
         sb.Begin();

         // Draw background
         sb.Draw(_bgTexture, new Rectangle(-30, -30, GraphicsDevice.Viewport.Width + 100, GraphicsDevice.Viewport.Height + 100), Color.White);

         for (Int32 x = 0; x < DungeonGrid.MaxWidth; x++)
         {
             for (Int32 y = 0; y < DungeonGrid.MaxHeight; y++)
             {
                 var tile = Dungeon.Tiles[x, y];

                 Vector2 position = new(
                     GameContext.DungeonPaddingX + GetColumnOffset(x),
                     GameContext.DungeonPaddingY + y * (GameContext.TileSize + GameContext.TilePadding)
                 );

                 if (tile.Type == DMTileType.Boss)
                 {
                     // Draw boss room directly
                     BossTexture ??= TextureManager.GetAnimation(DMObjectType.Room, "Boss", DMAnimationType.Static)[0].Texture;
                     position = new Vector2(
                         20,
                         GameContext.DungeonPaddingY + y * (GameContext.TileSize + GameContext.TilePadding) - (GameContext.TileSize * 0.125f) - 50
                     );

                     sb.Draw(BossTexture, position, null, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0f);
                 }
                 else
                 {
                     // Draw tile + units
                     tile.Draw(sb, position);
                 }
             }
         }
         
         foreach (var (tileA, tileB, isBossConnection, isEntranceConnection) in GetRoomConnections())
         {
             Vector2 posA, posB;
             Boolean flip = false;

             if (isBossConnection)
             {
                 posA = GetTileRightEdge(tileA);
                 posB = GetTileLeftEdge(tileB);
                 posA.X += 150;
                 posA.Y += 65;
             }
             else if (isEntranceConnection)
             {
                 posA = GetTileLeftEdge(tileA);
                 posB = GetTileRightEdge(tileB);
             }
             else if (tileA.GridPosition.X == tileB.GridPosition.X)
             {
                 if (tileA.GridPosition.Y < tileB.GridPosition.Y)
                 {
                     posA = GetTileBottomEdge(tileA);
                     posB = GetTileTopEdge(tileB);
                 }
                 else
                 {
                     posA = GetTileTopEdge(tileA);
                     posB = GetTileBottomEdge(tileB);
                     flip = true;
                 }
             }
             else
             {
                 if (tileA.GridPosition.X < tileB.GridPosition.X)
                 {
                     posA = GetTileRightEdge(tileA);
                     posB = GetTileLeftEdge(tileB);
                 }
                 else
                 {
                     posA = GetTileLeftEdge(tileA);
                     posB = GetTileRightEdge(tileB);
                     flip = true;
                 }
             }

             DrawConnectionLine(sb, posA, posB, 12f, flip);
         }
 
         sb.End();
         GraphicsDevice.SetRenderTarget(null);
     }
     
     private Vector2 GetTileScreenPosition(DungeonTile tile)
     {
         Int32 x = tile.GridPosition.X;
         Int32 y = tile.GridPosition.Y;

         if (tile.Type == DMTileType.Boss)
         {
             return new Vector2(
                 20, // your boss room X offset
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
             return 0; // Boss tile column, start at 0
         
         return (Int32)((column - 1) * (GameContext.TileSize + GameContext.TilePadding) + (GameContext.TileSize * 1.25f) + GameContext.TilePadding);
     }
     
     private void DrawConnectionLine(SpriteBatch sb, Vector2 start, Vector2 end, Single thickness, Boolean flip = false)
     {
         Vector2 edge = end - start;
         Single angle = (Single)Math.Atan2(edge.Y, edge.X);
         Single length = edge.Length();

         Single rotation = angle;
         if (flip)
             rotation += MathF.PI; // 180 degrees

         sb.Draw(_lineTexture, start, null, Color.White, rotation, Vector2.Zero,
             new Vector2(length / _lineTexture.Width, thickness / _lineTexture.Height),
             SpriteEffects.None, 0f);
     }
     
     private List<(DungeonTile, DungeonTile, Boolean, Boolean)> GetRoomConnections()
     {
         var connections = new List<(DungeonTile, DungeonTile, Boolean, Boolean)>();

         // Boss connection
         var bossTile = Dungeon.Tiles[0, DungeonGrid.MaxHeight / 2];
         var middleRoom = Dungeon.Tiles[1, DungeonGrid.MaxHeight / 2];
         connections.Add((bossTile, middleRoom, true, false));

         // Horizontal room connections
         for (Int32 x = 1; x < DungeonGrid.MaxWidth - 1; x++)
         {
             for (Int32 y = 0; y < DungeonGrid.MaxHeight; y++)
             {
                 var current = Dungeon.Tiles[x, y];
                 if (current.Type != DMTileType.RoomSlot)
                     continue;

                 var right = Dungeon.Tiles[x + 1, y];
                 if (right.Type == DMTileType.RoomSlot)
                     connections.Add((current, right, false, false));
             }
         }
         
         // Vertical room connections
         for (Int32 x = 1; x <= GameContext.Dungeon.ActiveRight; x++)
         {
             for (Int32 y = 0; y < DungeonGrid.MaxHeight - 1; y++)
             {
                 var current = Dungeon.Tiles[x, y];
                 var below = Dungeon.Tiles[x, y + 1];

                 if (current.Type != DMTileType.RoomSlot)
                     continue;
                 if (below.Type != DMTileType.RoomSlot)
                     continue;

                 connections.Add((current, below, false, false));
             }
         }
         
         // Entrance connections
         for (Int32 y = 0; y < DungeonGrid.MaxHeight; y++)
         {
             var entranceTile = Dungeon.Tiles[GameContext.Dungeon.ActiveRight + 1, y];
             if (entranceTile.Type != DMTileType.Entrance) 
                 continue;

             var leftTile = Dungeon.Tiles[GameContext.Dungeon.ActiveRight, y];
             if (leftTile.Type == DMTileType.RoomSlot)
                 connections.Add((entranceTile, leftTile, false, true));
         }

         return connections;
     }

     private Vector2 GetTileRightEdge(DungeonTile tile)
     {
         var pos = GetTileScreenPosition(tile);
         return new Vector2(
             pos.X + GameContext.TileSize,
             pos.Y + GameContext.TileSize / 2f
         );
     }

     private Vector2 GetTileLeftEdge(DungeonTile tile)
     {
         var pos = GetTileScreenPosition(tile);
         return new Vector2(
             pos.X,
             pos.Y + GameContext.TileSize / 2f
         );
     }
     
     private Vector2 GetTileTopEdge(DungeonTile tile)
     {
         var pos = GetTileScreenPosition(tile);
         return new Vector2(
             pos.X + GameContext.TileSize / 2f,
             pos.Y
         );
     }

     private Vector2 GetTileBottomEdge(DungeonTile tile)
     {
         var pos = GetTileScreenPosition(tile);
         return new Vector2(
             pos.X + GameContext.TileSize / 2f,
             pos.Y + GameContext.TileSize
         );
     }

 }