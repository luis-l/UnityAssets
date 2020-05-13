
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Steers away from objects to avoid collision.
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class ObstacleAvoidance2D : SteeringBehavior2D
    {
        [HideInInspector]
        [SerializeField]
        private ObstacleSensor2D _sensor;

        [HideInInspector]
        [SerializeField]
        private BoxCollider2D _sensorCollider;

        public enum ForwardAxis { RIGHT, UP };
        public ForwardAxis forward = ForwardAxis.RIGHT;

        public float minSensorSize = 1f;
        public float perpSensorSize = 1f;
        public float sensorOffset = -0.1f;

        private Collider2D _nearestObstacle;
        private Vector2 _nearestIntersectGlobal;

        /// <summary>
        /// How strong the agent should brake to avoid obstacles.
        /// </summary>
        public float brakingWeight = 0.1f;

        void Start()
        {
            _priority = Priority.HIGH;
        }

        public override Vector2 SteerForce()
        {
            getClosestObstacle(out _nearestObstacle);

            // Nothing detected, do not influence steering.
            if (_nearestObstacle == null) return Vector2.zero;

            Vector2 obstacleLocalPos = _sensor.World2Local(_nearestObstacle.bounds.center);
            float sensorLength = ForwardSensorSize;

            // The closer we are to the obstacle, the stronger the steering forces.
            float steerScalar = 1f + (sensorLength - obstacleLocalPos.x) / sensorLength;

            float radius = _nearestObstacle.GetBoundingRadius();
            float directionScalar = 1f;

            // If the object is above, make sure to steer downward.
            if (obstacleLocalPos.y > 0) {
                directionScalar = -1f;
            }

            float lateralForce = (directionScalar * radius - obstacleLocalPos.y) * steerScalar;
            float brakingForce = (radius - obstacleLocalPos.x) * brakingWeight;

            Vector2 localSteering = new Vector2(brakingForce, lateralForce);
            Vector2 global = _sensor.Local2WorldDirection(localSteering);

            return global;
        }

        void Update()
        {
            // Sync the sensor's direction with the agent's forward direction.
            if (!Agent.IsGraphicRotationSynced()) {
                _sensor.transform.rotation = Quaternion.Euler(0f, 0f, Agent.ForwardDegrees);
            }
        }

        void FixedUpdate()
        {
            // Sensor is proportional to the agent's speed.
            float sensorSize = minSensorSize + (Agent.rigid.velocity.magnitude / Agent.maxSpeed) * minSensorSize;

            // Since this keeps getting updated constantly, we do not need to worry
            // about changing members (ex. minSensorSize, perpSensorSize, sensorOffset)
            // because this function will update the collider with those values.
            setSensorDimensions(sensorSize);
        }

        private void setupSensor()
        {
            initSensor<ObstacleSensor2D, Collider2D, BoxCollider2D>(out _sensor, out _sensorCollider);
            setSensorDimensions(minSensorSize);
        }

        private void setSensorDimensions(float extension)
        {
            if (forward == ForwardAxis.UP) {
                _sensorCollider.size = new Vector2(perpSensorSize, extension);
                _sensorCollider.offset = new Vector2(0f, extension / 2f + sensorOffset);
            }

            else {
                _sensorCollider.size = new Vector2(extension, perpSensorSize);
                _sensorCollider.offset = new Vector2(extension / 2f + sensorOffset, 0f);
            }
        }

        /// <summary>
        /// The extent perpendicular to the forward direction.
        /// </summary>
        public float PerpendicularSensorSize
        {
            get
            {
                /* 
                 * NOTE:
                 * We multiply the size with the transform scale
                 * in order to the box size in world space.
                */

                // The forward is in the Y axis, so return X as the right extent.
                if (forward == ForwardAxis.UP) {
                    return _sensorCollider.size.x * transform.localScale.x;
                }

                // The forward is in the X axis, so return Y as the right extent.
                return _sensorCollider.size.y * transform.localScale.y;
            }
        }

        /// <summary>
        /// The extent in the same direction as the forward direction.
        /// </summary>
        public float ForwardSensorSize
        {
            get
            {
                if (forward == ForwardAxis.UP) {
                    return _sensorCollider.size.y;
                }

                return _sensorCollider.size.x;
            }
        }

        // The bounds used to check for intersection points between agent and obstacles. 
        private float getExtendedBoundsRadius(Collider2D collider)
        {
            return collider.GetBoundingRadius() + PerpendicularSensorSize / 2f;
        }

        // Compute the intersecting point between the x axis and the circle
        // and return the one closest to the local origin.
        private static float xAxisIntersectionLocal(Vector2 localCenter, float radius)
        {
            float cy = localCenter.y;
            float sq = Mathf.Sqrt(radius * radius - cy * cy);

            float cx = localCenter.x;
            float x = cx - sq;

            if (x <= 0) {
                x = cx + sq;
            }

            return x;
        }

        // Get the closest obstacle in the sensor relative to the Agent's position.
        private void getClosestObstacle(out Collider2D closest)
        {
            closest = null;

            // The x-coordinate (in the sensor's local space) of the 
            // closest bounding circle (the bounds of the collider detected).
            float closestIntersectX = float.MaxValue;

            foreach (Collider2D other in _sensor.Detected) {

                // Create the bounding circle for the detected collider.
                Vector2 boundsLocalCenter = _sensor.World2Local(other.bounds.center);
                float boundsRadius = getExtendedBoundsRadius(other);

                // Calculate the closest x-intersection between the sensor's x-axis and the bounding circle.
                float intersectX = xAxisIntersectionLocal(boundsLocalCenter, boundsRadius);

                // Update the closest x-intersection if needed.
                if (intersectX < closestIntersectX) {

                    closestIntersectX = intersectX;
                    closest = other;

                    // The intersection point in world space.
                    _nearestIntersectGlobal = _sensor.Local2World(closestIntersectX, 0f);
                }
            }
        }

        void OnDestroy()
        {
            DestroyImmediate(_sensor.gameObject);
        }

        void OnValidate()
        {
            setSensorDimensions(minSensorSize);
        }

        void Reset()
        {
            setupSensor();
        }

        void OnDrawGizmosSelected()
        {
            if (_nearestObstacle != null) {

                Vector2 sensorPosition = _sensor.transform.position;

                DrawUtil.DrawLine(sensorPosition, _nearestIntersectGlobal, Color.red);
                DrawUtil.DrawCrosshair(_nearestIntersectGlobal, 1f, Color.red, 0f);

                Vector2 obstacleCenter = _nearestObstacle.bounds.center;
                float obstacleRadius = getExtendedBoundsRadius(_nearestObstacle);

                // Draw obstacle
                DrawUtil.DrawCircle(obstacleCenter, obstacleRadius, Color.red);
                DrawUtil.DrawCrosshair(obstacleCenter, 0.3f, Color.red);
            }
        }
    }
}
