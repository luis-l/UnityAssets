
using UnityEngine;

using Bonsai.Core;

public class Wander : Task
{
    public float speed = 1f;

    public Vector2 wanderOrigin = Vector2.zero;
    public float wanderRange = 8f;

    [Tooltip("The rate to switch to a new location to wander to.")]
    public float wanderChangeRate = 1f;

    private float _wanderChangeCounter = 0f;

    private Transform _transform;
    private Vector2 _targetPos;

    public override void OnStart()
    {
        _transform = GameObjectParent.transform;
    }

    public override void OnEnter()
    {
        _wanderChangeCounter = 0f;
        setWanderTarget();
    }

    public override BehaviourNode.Status Run()
    {
        _wanderChangeCounter += Time.deltaTime;

        // Update the wander position.
        if (_wanderChangeCounter > wanderChangeRate) {
            setWanderTarget();
            _wanderChangeCounter = 0f;
        }

        // Lerp towards the position we want wander to.
        _transform.position = Vector3.Lerp(_transform.position, _targetPos, Time.deltaTime * speed);

        return Status.Running;
    }

    private void setWanderTarget()
    {
        // Pick a random location within the wander circle
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _targetPos = wanderOrigin + randomDirection * Random.Range(0.1f, wanderRange);
    }
}
