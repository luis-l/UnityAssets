
using System.Collections.Generic;
using UnityEngine;

using Bonsai.Core;

public class FindTarget : ConditionalAbort {

    public float sensorRange = 5f;

    [Tooltip("The blackboard variable that stores the transform target found.")]
    public string targetKey;

    [Tooltip("How many colliders to detect at once.")]
    public int sensorCacheLimit = 5;

    [Tooltip("The mask of the colliders to detect.")]
    public LayerMask detectionLayer;

    private Collider2D[] _collidersDetected;

    public override void OnStart()
    {
        _collidersDetected = new Collider2D[sensorCacheLimit];
    }

    public override bool Condition()
    {
        Vector3 pos = GameObjectParent.transform.position;

        // Perform a circle cast to find colliders.
        int hitCount = Physics2D.OverlapCircleNonAlloc(pos, sensorRange, _collidersDetected, detectionLayer.value);

        if (hitCount == 0) {
            return false;
        }

        int indexOfNearest = 0;
        float nearestDistanceSq = float.MaxValue;

        // Find the nearest collider
        for (int i = 0; i < hitCount; ++i) {

            Collider2D col = _collidersDetected[i];

            float distanceSq = (col.transform.position - pos).sqrMagnitude;

            if (distanceSq < nearestDistanceSq) {
                nearestDistanceSq = distanceSq;
                indexOfNearest = i;
            }
        }

        Transform target = _collidersDetected[indexOfNearest].transform;

        // Store in the blackboard.
        Blackboard.Set(targetKey, target);

        return true;
    }

    public override void OnDrawGizmos()
    {
        AutonomousAgents.DrawUtil.DrawCircle(GameObjectParent.transform.position, sensorRange, Color.green);
    }
}
