using System;

namespace DungeonMaker;

public enum DMObjectType
{
    Room,
    Monster,
    Hero,
    // Unit = Monster | Hero,
    Skill, 
    Relic,
    Equipment,
    Status
}