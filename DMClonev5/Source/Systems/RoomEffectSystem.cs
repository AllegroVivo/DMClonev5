using System;
using System.Collections.Generic;
using DungeonMaker.Components;
using DungeonMaker.Core;
using DungeonMaker.Entities;
using DungeonMaker.Events;
using DungeonMaker.Objects;
using DungeonMaker.Utilities;

namespace DungeonMaker.Systems;

public class RoomEffectSystem : SystemBase
{
    private static EntityManager EntityManager => GameContext.EntityManager;

    public RoomEffectSystem()
    {
        EventBus.Subscribe<MonsterDeployedEvent>(OnMonsterDeployed);
    }

    public override void Update()
    {
    }

    private void OnMonsterDeployed(MonsterDeployedEvent e)
    {
        if (!EntityManager.TryGetComponent<BattleRoomComponent>(e.Room, out var roomComponent))
            return;

        foreach (var effect in roomComponent.Effects)
        {
            if (effect.Trigger != DMEventTrigger.OnMonsterDeploy)
                continue;

            ApplyEffect(effect, e.Monster, e.Room);
        }
    }

    private void ApplyEffect(EffectData effect, Entity targetEntity, Entity roomEntity)
    {
        switch (effect.Action)
        {
            case DMActionType.BoostStat:
                ApplyStatBoost(effect, targetEntity, roomEntity);
                break;

            // TODO: Handle other effect types

            default:
                Logger.Error($"Unknown effect action: {effect.Action}");
                break;
        }
    }

    private void ApplyStatBoost(EffectData effect, Entity targetEntity, Entity roomEntity)
    {
        if (!EntityManager.TryGetComponent<MonsterStatsComponent>(targetEntity, out var stats))
            return;

        if (!effect.Parameters.TryGetValue("Stat", out var statName) ||
            !Enum.TryParse<DMStatType>((String)statName, true, out var statType))
        {
            Logger.Warning($"Invalid or missing stat name in effect parameters: {statName}");
            return;
        }

        if (!effect.Parameters.TryGetValue("Value", out var baseValueStr) ||
            !TryConvertToFloat(baseValueStr, out Single baseValue))
        {
            Logger.Warning($"Invalid or missing base value in effect parameters: {baseValueStr}");
            return;
        }

        Single totalValue = baseValue;

        if (effect.Parameters.TryGetValue("PerLevel", out var perLevelStr) &&
            TryConvertToFloat(perLevelStr, out Single perLevel))
        {
            Int32 level = 0;
            if (EntityManager.TryGetComponent<LevelComponent>(targetEntity, out var levelComponent))
                level = levelComponent.Current;

            totalValue += perLevel * level;
        }

        var modification = new DMStatModification(totalValue, roomEntity);
        stats[statType].QueueModification(modification);

        var objRef = EntityManager.GetComponent<DMObjectReference>(targetEntity);
        Logger.Debug($"Applied {effect.Action} effect: {statType} +{totalValue} to {objRef.Source.Name}");
    }
    
    private static Boolean TryConvertToFloat(Object? obj, out Single value)
    {
        try
        {
            value = Convert.ToSingle(obj);
            return true;
        }
        catch
        {
            value = 0f;
            return false;
        }
    }

}
