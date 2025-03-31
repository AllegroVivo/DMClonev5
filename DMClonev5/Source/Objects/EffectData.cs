using System;
using System.Collections.Generic;

namespace DungeonMaker.Objects;

public class EffectData
{
    public DMEventTrigger Trigger { get; set; }
    public DMActionType Action { get; set; }
    public DMTargetType Target { get; set; }
    public Dictionary<String, Object> Parameters { get; set; } = new();
}