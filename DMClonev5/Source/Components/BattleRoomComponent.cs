using System;
using System.Collections.Generic;
using DungeonMaker.Objects;

namespace DungeonMaker.Components;

public class BattleRoomComponent : IComponent
{
    public List<EffectData> Effects { get; set; } = [];
}