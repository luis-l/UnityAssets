
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steers the agent behind cover to hide from a target.
    /// </summary>
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Hide2D : SteeringBehavior2D
    {

        [SerializeField]
        private float _obstacleSensorRadius = 10f;

        [Tooltip("How far behind should the agent hide behind an obstacle.")]
        public float hideOffsetFromObstacle = 2f;

        [Tooltip("The sensor to dicate what target to evade and hide from.")]
        public EvadeSensor2D evadeSensor;

        [Tooltip("The sensor to dictate how fast to approach the hiding position.")]
        public ArriveSensor2D arriveSensor;

        [HideInInspector]
        [SerializeField]
        private ObstacleSensor2D _obstacleSensor;

        [HideInInspector]
        [SerializeField]
        private CircleCollider2D _obstacleSensorCollider;

        private Vector2 _lastHidingSpot;

        void Start()
        {
            _lastHidingSpot = Agent.transform.position;
        }

        public override Vector2 SteerForce()
        {
            if (!evadeSensor.ShouldAvoid(Agent)) {
                arriveSensor.target = _lastHidingSpot;
                return Arrive2D.SteerForce(Agent, arriveSensor);
            }

            Vector2 hidingSpot = Vector2.zero;

            // Arrive at the hiding position.
            if (getNearestHidingSpot(ref hidingSpot)) {

                _lastHidingSpot = hidingSpot;
                arriveSensor.target = hidingSpot;
                return Arrive2D.SteerForce(Agent, arriveSensor);
            }

            // No hiding position available, evade target.
            else {
                return Evade2D.SteerForce(Agent, evadeSensor);
            }
        }

        private void setupSensor()
        {
            initSensor<ObstacleSensor2D, Collider2D, CircleCollider2D>(out _obstacleSensor, out _obstacleSensorCollider);
            setSensorDimensions(_obstacleSensorRadius);
        }

        private void setSensorDimensions(float radius)
        {
            if (_obstacleSensorCollider != null) {
                _obstacleSensorCollider.radius = radius;
            }

            else {
                Debug.LogError("Sensor does not exist!");
            }
        }

        private bool getNearestHidingSpot(ref Vector2 hidingSpot)
        {
            float closestHidingSpotSoFar = float.MaxValue;

            foreach (Collider2D detected in _obstacleSensor.Detected) {

                Vector2 candidateHidingSpot = getHidingPosition(detected);
                float distanceToHidingSpot = Vector2.Distance(Agent.transform.position, candidateHidingSpot);

                // Update the closest hiding spot if needed.
                if (distanceToHidingSpot < closestHidingSpotSoFar) {
                    closestHidingSpotSoFar = distanceToHidingSpot;
                    hidingSpot = candidateHidingSpot;
                }
            }

            // If nothing is found, then the distance to the cloest
            // hiding spot will be "infinity".
            return closestHidingSpotSoFar != float.MaxValue;
        }

        private Vector2 getHidingPosition(Collider2D collider)
        {
            Vector2 obstaclePosition = collider.transform.position;
            Vector2 targetPosition = evadeSensor.avoid.position;

            Vector2 toObjectFromTarget = obstaclePosition - targetPosition;
            toObjectFromTarget.Normalize();

            float hideDistanceFromObstacle = collider.GetBoundingRadius() + hideOffsetFromObstacle;

            Vector2 hidingSpot = (toObjectFromTarget * hideDistanceFromObstacle) + obstaclePosition;
            return hidingSpot;
        }

        public float SensorRadius
        {
            get { return _obstacleSensorRadius; }
            set
            {
                if (value <= 0f) {
                    Debug.LogWarning("Invalid radius: " + value);
                }

                else {
                    _obstacleSensorRadius = value;
                    setSensorDimensions(_obstacleSensorRadius);
                }
            }
        }

        void OnDestroy()
        {
            DestroyImmediate(_obstacleSensor.gameObject);
        }

        void OnValidate()
        {
            setSensorDimensions(_obstacleSensorRadius);
            arriveSensor.target = transform.position;
        }

        void Reset()
        {
            setupSensor();
        }

        void OnDrawGizmosSelected()
        {
            Evade2D.DrawGizmos(Agent, evadeSensor);
            Arrive2D.DrawGizmos(Agent, arriveSensor);

            // Draw the obstacle sensor dimensions
            DrawUtil.DrawCircle(_obstacleSensor.transform.position, _obstacleSensorCollider.radius, Color.yellow);
        }
    }
}