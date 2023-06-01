using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver;

public class IsThreatened : WorldKeyBase
{

}

public class IsEnemyDead : WorldKeyBase
{

}

public class WanderTarget : TargetKeyBase
{

}

public class IsIdle : WorldKeyBase
{

}

public class TurretSetConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        var builder = new GoapSetBuilder("GettingStartedSet");

        // Goals
        builder.AddGoal<IdleGoal>()
            .AddCondition<IsIdle>(Comparison.GreaterThanOrEqual, 1);

        builder.AddGoal<DestroyEnemyGoal>()
            .AddCondition<IsEnemyDead>(Comparison.GreaterThanOrEqual, 1);

        // Actions
        builder.AddAction<IdleAction>()
            .AddEffect<IsIdle>(true)
            .SetBaseCost(1);

        builder.AddAction<LookTowardsEnemyAction>()
            .AddEffect<IsThreatened>(true)
            .SetBaseCost(1);

        builder.AddAction<ShootEnemyAction>()
            .AddCondition<IsThreatened>(Comparison.GreaterThanOrEqual, 1)
            .AddEffect<IsEnemyDead>(true)
            .SetBaseCost(1);

        // Target Sensors
        //builder.AddTargetSensor<WanderTargetSensor>()
        //    .SetTarget<WanderTarget>();

        // World Sensors
        builder.AddWorldSensor<IsThreatenedSensor>()
            .SetKey<IsThreatened>();
        builder.AddWorldSensor<IsEnemyDeadSensor>()
            .SetKey<IsEnemyDead>();

        return builder.Build();
    }
}
