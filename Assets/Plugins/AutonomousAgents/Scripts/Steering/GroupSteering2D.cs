
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// Base class for steering behaviors dependent on groups of agents.
    /// </summary>
    public abstract class GroupSteering2D : SteeringBehavior2D
    {
        public float sensorRadius = 3f;

        [SerializeField]
        int _colliderCacheSize = 20;

        protected Collider2D[] _agentColliders;

        private int _layerMask;

        protected void Awake()
        {
            _agentColliders = new Collider2D[_colliderCacheSize];
            _layerMask = LayerMask.GetMask("Agent");
        }

        protected int GetNeighbors()
        {
            return Physics2D.OverlapCircleNonAlloc(Agent.transform.position, sensorRadius, _agentColliders, _layerMask);
        }

        protected void drawSensorBounds()
        {
            DrawUtil.DrawCircle(Agent.transform.position, sensorRadius, Color.red);
        }
    }
}