using UnityEngine;

public enum TrajectoryFunctions {
    None,
    Sine,
    Cosine,
    X,
    XSquared
}

public interface ITrajectory 
{
    public TrajectoryFunctions HorizontalTrajectory { get; }
    public TrajectoryFunctions VerticalTrajectory { get; }
    public TrajectoryFunctions ForwardTrajectory { get; }

    public float CalculateHorizontalScaleDelta(float timeStep);
    public float CalculateVerticalScaleDelta(float timeStep);
    public float CalculateForwardScaleDelta(float timeStep);
    public float InitializeForwardStartDir();
    public float InitializeVerticalStartDir();
    public float InitializeHorizontalStartDir();
    public float InitializeVerticalStartSpeed();
    public float InitializeHorizontalStartSpeed();
    public float InitializeForwardStartSpeed();
    public Vector3 CalculateTrajectory(Vector3 forward, Vector3 up, Vector3 right, float timeStep, 
            float horizontalScaleDelta, float verticalScaleDelta, float forwardScaleDelta,
            float verticalSpeed, float horizontalSpeed, float forwardSpeed, bool reflected);
}
