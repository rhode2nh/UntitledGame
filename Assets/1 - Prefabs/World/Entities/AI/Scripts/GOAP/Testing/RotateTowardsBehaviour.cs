using UnityEngine;

public class RotateTowardsBehaviour : MonoBehaviour
{
    public GameObject barrel;
    public GameObject shell;
    public float rotateSpeed;
    private float maxAngle = 45.0f;

    public void RotateTowardsEnemy(Vector3 enemyPos)
    {
        var targetDirection = enemyPos - barrel.transform.position;
        var targetRotation = Quaternion.LookRotation(transform.up, -targetDirection) * Quaternion.AngleAxis(90f, Vector3.right);
        Quaternion targetBarrelRotation;
        if (shell.transform.rotation == targetRotation && Vector3.Angle(transform.up, barrel.transform.forward) <= 90)
        {
            targetBarrelRotation = Quaternion.LookRotation(targetDirection, transform.up);
            barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetBarrelRotation, rotateSpeed * Time.deltaTime);
        }
        shell.transform.rotation = Quaternion.RotateTowards(shell.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
