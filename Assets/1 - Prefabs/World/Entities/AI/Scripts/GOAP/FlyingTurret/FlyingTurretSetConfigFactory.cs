using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

public class FlyingTurretSetConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create() {
        var builder = new GoapSetBuilder("FlyingTurretAISet");

        // Goals
        builder.AddGoal<WanderGoal>()
            .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);
        builder.AddGoal<DestroyEnemyGoal>()
            .AddCondition<IsEnemyDead>(Comparison.GreaterThanOrEqual, 1);

        // Actions
        builder.AddAction<WanderAction>()
            .SetTarget<WanderTarget>()
            .AddEffect<IsWandering>(true)
            .SetBaseCost(1)
            .SetInRange(4f);

        builder.AddAction<LookAtAction>()
            .AddCondition<IsThreatened>(Comparison.GreaterThanOrEqual, 1)
            .AddEffect<IsEnemyDead>(true)
            .SetBaseCost(1);

        // Target Sensors
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        builder.AddWorldSensor<IsThreatenedSensor>()
            .SetKey<IsThreatened>();
    
        return builder.Build();
    }
    
}
