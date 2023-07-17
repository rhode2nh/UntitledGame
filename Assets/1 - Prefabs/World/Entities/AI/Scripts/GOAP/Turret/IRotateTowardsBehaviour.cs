using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotateTowardsBehaviour 
{
    void RotateTowardsEnemy(Vector3 enemyPos);

    void GoToPosition(Vector3 position);

    void StopGoingToPosition();
}
