using UnityEngine;

public class AgentRotateTowardsBehaviour : MonoBehaviour, IRotateTowardsBehaviour
{
    public float rotateSpeed;
    public GameObject body;

    public void RotateTowardsEnemy(Vector3 enemyPos)
    {
        var targetDirection = enemyPos - body.transform.position;
        var targetRotation = Quaternion.LookRotation(transform.up, -targetDirection) * Quaternion.AngleAxis(90f, Vector3.right);
        //Quaternion targetBarrelRotation;
        //if (shell.transform.rotation == targetRotation && Vector3.Angle(transform.up, barrel.transform.forward) <= 90)
        //{
        //    targetBarrelRotation = Quaternion.LookRotation(targetDirection, transform.up);
        //    barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, targetBarrelRotation, rotateSpeed * Time.deltaTime);
        //}
        body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
