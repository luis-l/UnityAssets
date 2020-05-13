
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Base class of all objects requiring basic agent components.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent2D : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D rigid;

        /// <summary>
        /// The maximum speed which the agent may move.
        /// </summary>
        public float maxSpeed = 3f;

        /// <summary>
        /// The maximum force the agent may thrust itself.
        /// </summary>
        public float maxForce = 20f;

        /// <summary>
        /// The maximum turning rate of the agent.
        /// </summary>
        public float maxRotationRate = 3f;

        private Vector2 _forwardDirection;

        /// <summary>
        /// The graphical representation of the agent.
        /// </summary>
        public Transform graphicTransform;

        /// <summary>
        /// The rotation offset of the graphical representation.
        /// </summary>
        public float graphicAngleOffset = 0f;

        private float _forwardDegrees;

        protected void Awake()
        {
            setupRigid();
            setupGraphic();
        }

        protected virtual void Update()
        {
            // Update the forward direction if we are still moving.
            if (!rigid.velocity.NearZero()) {
                _forwardDirection = rigid.velocity.normalized;
                updateForwardDegrees();
            }

            alignGraphicToVelocity();
        }

        /// <summary>
        /// The velocity direction of the agent.
        /// </summary>
        public Vector2 ForwardDirection
        {
            get
            {
                // Align to current direction if invalid.
                if (_forwardDirection == Vector2.zero) {
                    _forwardDirection = transform.Orientation();
                }

                return _forwardDirection;
            }
        }

        /// <summary>
        /// The perpendicular direction of the agent's velocity.
        /// </summary>
        public Vector2 RightDirection
        {
            get { return _forwardDirection.Perpendicular(); }
        }

        protected void capVelocity()
        {
            if (rigid.velocity.sqrMagnitude >= maxSpeed * maxSpeed) {
                rigid.velocity = rigid.velocity.normalized * maxSpeed;
            }
        }

        // Aligns the graphical representation along the forward direction.
        private void alignGraphicToVelocity()
        {
            if (graphicTransform != null) {

                // No need to slerp if already aligned to velocity direction.
                if (IsGraphicRotationSynced()) return;

                float degrees = getGraphicRotation();

                // Slerp between the current rotation and velocity direction.
                var rotA = graphicTransform.rotation;
                var rotB = Quaternion.Euler(0, 0, degrees);

                float slerpSpeed = maxRotationRate * Time.deltaTime;

                graphicTransform.rotation = Quaternion.Slerp(rotA, rotB, slerpSpeed);
            }
        }

        private float getGraphicRotation()
        {
            return ForwardDegrees + graphicAngleOffset;
        }

        // Compute the degrees of the ForwardDirection.
        private void updateForwardDegrees()
        {
            float angle = Mathf.Atan2(ForwardDirection.y, ForwardDirection.x);

            // Cache in order to avoid recomputation of trig functions for public access.
            _forwardDegrees = Mathf.Rad2Deg * angle;
        }

        /// <summary>
        /// Get the ForwardDirection in degrees.
        /// </summary>
        public float ForwardDegrees
        {
            get
            {
                return _forwardDegrees;
            }
        }

        /// <summary>
        /// If the graphical representation orientation is the same as the ForwardDirection.
        /// </summary>
        /// <returns></returns>
        public bool IsGraphicRotationSynced()
        {
            if (graphicTransform == null) return false;

            Vector2 graphicDir = graphicTransform.Orientation();

            return graphicDir.NormalizedEqual(ForwardDirection);
        }

        void OnDrawGizmosSelected()
        {
            // Draw the agent velocity.
            if (rigid != null && !rigid.velocity.IsZero()) {
                Vector2 velStart = transform.Position2D();
                Vector2 velEnd = transform.Position2D() + rigid.velocity;
                DrawUtil.DrawArrow(velStart, velEnd, Color.blue, 0.5f);
            }
        }

        // This code sets up a default graphic representation for the agent.
        public const string kDefaultSpritePath = "Sprites/Triangle";
        public const string kGraphicName = "Graphic";
        private void setupGraphic()
        {
            graphicTransform = transform.FindChild(kGraphicName);

            // Graphic already added to the agent.
            if (graphicTransform != null) {
                graphicTransform.localPosition = Vector3.zero;
                return;
            }

            GameObject graphicGameObject = new GameObject(kGraphicName);

            graphicTransform = graphicGameObject.transform;
            graphicTransform.parent = transform;
            graphicTransform.localPosition = Vector3.zero;

            var renderer = graphicGameObject.AddComponent<SpriteRenderer>();

            var sprite = Resources.Load<Sprite>(kDefaultSpritePath);

            // Setup sprite if we found some default assets.
            if (sprite != null) {
                renderer.sprite = sprite;

                float hue = Random.value;
                float sat = Random.Range(0.6f, 1f);

                renderer.color = Color.HSVToRGB(hue, sat, 1f);
                graphicAngleOffset = 270f;
            }

            else {
                Debug.LogWarning("Failed to find default sprite path: " + kDefaultSpritePath);
            }
        }

        protected void setupRigid()
        {
            rigid = GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        protected void Reset()
        {
            setupGraphic();
            setupRigid();
        }

        public override int GetHashCode()
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            return x + y * GetInstanceID();
        }
    }
}