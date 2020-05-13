
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerNet : NetworkBehaviour 
{
    public float maxSpeed = 3f;
    public float acceleration = 3f;

    private Rigidbody2D _rigid;

    private float _vInput, _hInput;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer) {
            Destroy(this);
            return;
        }

        _rigid = GetComponent<Rigidbody2D>();

        _rigid.gravityScale = 0f;
        _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        _vInput = Input.GetAxisRaw("Vertical");
        _hInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        Vector2 force = (new Vector2(_hInput, _vInput));

        _rigid.AddForce(force.normalized * acceleration);

        // Cap max velocity
        if (_rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed) {
            _rigid.velocity = _rigid.velocity.normalized * maxSpeed;
        }
    }
}
