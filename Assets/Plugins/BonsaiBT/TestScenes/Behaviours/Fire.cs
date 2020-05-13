
using UnityEngine;
using UnityEngine.Networking;

using Bonsai.Core;

public class Fire : Task  {

    public float firingRate = 0.2f;
    public float firingForce = 20f;
    public GameObject projectilePrefab;

    public string targetKey;

    private float _firingCounter = 0f;
    private Transform _target;
    private Transform _transform;

    public override void OnStart()
    {
        _transform = GameObjectParent.transform;
    }

    public override void OnEnter()
    {
        _target = Blackboard.Get<Transform>(targetKey);
    }

    public override Status Run()
    {
        if (_target == null) {
            return Status.Failure;
        }

        _firingCounter += Time.deltaTime;

        if (_firingCounter > firingRate) {
            fireProjectile();
            _firingCounter = 0f;
        }

        return Status.Running;
    }

    private void fireProjectile()
    {
        var proj = GameObject.Instantiate(projectilePrefab);

        // Position the projectile at the game object's position.
        proj.transform.position = _transform.position;

        Vector2 toTarget = (_target.position - _transform.position).normalized;

        // Add force to the projectile.
        var rigid = proj.GetComponent<Rigidbody2D>();
        rigid.AddForce(toTarget * firingForce, ForceMode2D.Impulse);

        NetworkServer.Spawn(proj);
    }
}
