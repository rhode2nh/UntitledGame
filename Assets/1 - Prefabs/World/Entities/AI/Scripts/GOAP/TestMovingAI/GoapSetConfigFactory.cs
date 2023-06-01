using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver;

public class IsWandering : WorldKeyBase {}

public class GoapSetConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        var builder = new GoapSetBuilder("TestMovingAISet");

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
            .SetInRange(1f);

        builder.AddAction<LookTowardsEnemyAction>()
            .AddEffect<IsThreatened>(true)
            .SetBaseCost(1);

        builder.AddAction<ShootEnemyAction>()
            .AddCondition<IsThreatened>(Comparison.GreaterThanOrEqual, 1)
            .AddEffect<IsEnemyDead>(true)
            .SetBaseCost(1);

        // Target Sensors
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget<WanderTarget>();

        // World Sensors
        builder.AddWorldSensor<IsThreatenedSensor>()
            .SetKey<IsThreatened>();
        builder.AddWorldSensor<IsEnemyDeadSensor>()
            .SetKey<IsEnemyDead>();

        return builder.Build();
    }
}
