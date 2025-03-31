using System;
using DungeonMaker.Objects;

namespace DungeonMaker.Components;

public sealed class DMObjectReference(DMObject source) : IComponent
{
    public DMObject Source { get; } = source;
}