using System;

namespace DungeonMaker.Core;

public class SpriteMetadata
{
    public RectData Rect { get; set; }
    public OffsetData Offset { get; set; }
}

public class RectData
{
    public Single X { get; set; }
    public Single Y { get; set; }
    public Single Width { get; set; }
    public Single Height { get; set; }
}

public class OffsetData
{
    public Single X { get; set; }
    public Single Y { get; set; }
}