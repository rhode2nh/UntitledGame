using UnityEngine;

[CreateAssetMenu(fileName = "Trajectory", menuName = "Items/Modifiers/New Trajectory", order = 1)]
public class Trajectory : Modifier, ITrajectory
{
    [Space]
    [SerializeField]
    private TrajectoryFunctions forwardTrajectory;
    [SerializeField]
    private TrajectoryFunctions verticalTrajectory;
    [SerializeField]
    private TrajectoryFunctions horizontalTrajectory;
    [Space]
    [SerializeField]
    private float forwardScalar;
    [SerializeField]
    private float verticalScalar;
    [SerializeField]
    private float horizontalScalar;
    [Space]
    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private float verticalSpeed;
    [SerializeField]
    private float horizontalSpeed;
    [Space]
    [SerializeField]
    private bool lerpForwardScalar;
    [SerializeField]
    private bool lerpVerticalScalar;
    [SerializeField]
    private bool lerpHorizontalScalar;
    [Space]
    [SerializeField]
    private float forwardScaleRate;
    [SerializeField]
    private float verticalScaleRate;
    [SerializeField]
    private float horizontalScaleRate;
    [Space]
    [SerializeField]
    private bool randomizeForwardDir;
    [SerializeField]
    private bool randomizeVerticalDir;
    [SerializeField]
    private bool randomizeHorizontalDir;
    [Space]
    [SerializeField]
    private bool randomizeForwardStartSpeed;
    [SerializeField]
    private bool randomizeVerticalStartSpeed;
    [SerializeField]
    private bool randomizeHorizontalStartSpeed;

    public new void Awake() 
    {
        this.Name = Constants.MOD_TRAJECTORY;
        this.isStackable = false;
    }

    public TrajectoryFunctions HorizontalTrajectory { get => horizontalTrajectory; }
    public TrajectoryFunctions VerticalTrajectory { get => verticalTrajectory; }
    public TrajectoryFunctions ForwardTrajectory { get => forwardTrajectory; }

    public float InitializeForwardStartDir() {
        float[] randomStartDir = new float[2] { -1.0f, 1.0f };
        if (randomizeForwardDir) {
            return randomStartDir[UnityEngine.Random.Range(0, 2)];
        } else {
            return 1.0f;
        }
    }

    public float InitializeVerticalStartDir() {
        float[] randomStartDir = new float[2] { -1.0f, 1.0f };
        if (randomizeVerticalDir) {
            return randomStartDir[Random.Range(0, 2)];
        } else {
            return 1.0f;
        }
    }

    public float InitializeHorizontalStartDir() {
        float[] randomStartDir = new float[2] { -1.0f, 1.0f };
        if (randomizeHorizontalDir) {
            return randomStartDir[UnityEngine.Random.Range(0, 2)];
        } else {
            return 1.0f;
        }
    }

    public float InitializeVerticalStartSpeed() {
        if (!randomizeVerticalStartSpeed) {
            return verticalSpeed;
        } else {
            return Random.Range(1.0f, verticalSpeed);
        }
    }

    public float InitializeHorizontalStartSpeed() {
        if (!randomizeHorizontalStartSpeed) {
            return horizontalSpeed;
        } else {
            return Random.Range(1.0f, horizontalSpeed);
        }
    }

    public float InitializeForwardStartSpeed() {
        if (!randomizeForwardStartSpeed) {
            return forwardSpeed;
        } else {
            return Random.Range(1.0f, forwardSpeed);
        }
    }

    public float CalculateVerticalScaleDelta(float timeElapsed) {
        if (!lerpVerticalScalar || verticalScaleRate * timeElapsed > verticalScalar) {
            return verticalScalar;
        }
        return Mathf.Lerp(0, verticalScalar, verticalScaleRate * timeElapsed);
    }
    
    public float CalculateForwardScaleDelta(float timeElapsed) {
        if (!lerpForwardScalar || forwardScaleRate * timeElapsed > forwardScalar) {
            return forwardScalar;
        }
        return Mathf.Lerp(0, forwardScalar, forwardScaleRate * timeElapsed);
    }

    public float CalculateHorizontalScaleDelta(float timeElapsed) {
        if (!lerpHorizontalScalar || horizontalScaleRate * timeElapsed > horizontalScalar) {
            return horizontalScalar;
        }
        return Mathf.Lerp(0, horizontalScalar, horizontalScaleRate * timeElapsed);
    }

    public Vector3 CalculateTrajectory(Vector3 forward, Vector3 up, Vector3 right, float timeStep, float horizontalScaleDelta, float verticalScaleDelta, float forwardScaleDelta, float verticalSpeed, float horizontalSpeed, float forwardSpeed, bool reflected) {
        float forwardStep = CalculateTrajectoryFunction(ForwardTrajectory, timeStep, forwardSpeed, reflected);
        float horizontalStep = CalculateTrajectoryFunction(HorizontalTrajectory, timeStep, horizontalSpeed, reflected);
        float verticalStep = CalculateTrajectoryFunction(VerticalTrajectory, timeStep, verticalSpeed, reflected);
        return (forward * forwardStep * (forwardScalar * forwardScaleDelta))
        + (up * verticalStep * (verticalScalar * verticalScaleDelta))
        + (right * horizontalStep * (horizontalScalar * horizontalScaleDelta));
    }

    private float CalculateTrajectoryFunction(TrajectoryFunctions function, float timeStep, float speed, bool reflected) {
        switch(function) {
            case TrajectoryFunctions.Sine:
                return Mathf.Sin(timeStep * speed);
            case TrajectoryFunctions.Cosine:
                if (reflected) {
                    return 1f - Mathf.Cos(timeStep * speed);
                }
                return Mathf.Cos(timeStep * speed);
            case TrajectoryFunctions.X:
                return timeStep * speed;
            case TrajectoryFunctions.XSquared:
                return Mathf.Pow(timeStep * speed, 2);
            default:
                return 0.0f;
        }
    }
}
