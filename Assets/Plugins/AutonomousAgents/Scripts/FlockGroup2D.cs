
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A manager class to help setup and modify a group of flocking agents.
    /// </summary>
    public class FlockGroup2D : MonoBehaviour
    {

        [SerializeField]
        private int _flockSize = 20;

        [SerializeField]
        private float _separateScale = 1f;

        [SerializeField]
        private float _alignScale = 1f;

        [SerializeField]
        private float _cohesionScale = 1f;

        public bool useSamplePath = false;

        private List<Flocking2D> _flockBehaviors = new List<Flocking2D>();

        void Awake()
        {
            SpawnFlock();
            updateFlockingParameters();
        }

        private void SpawnFlock()
        {
            Vector2[] waypoints = new Vector2[2];
            if (useSamplePath) {
                waypoints = Path2D.BoxWaypoints(transform.position, 30f, 20f);
            }

            for (int i = 0; i < _flockSize; ++i) {
                setupFlockingAgent(waypoints, i);
            }

            updateFlockingParameters();
        }

        private void setupFlockingAgent(Vector2[] waypoints, int i)
        {
            GameObject agentGO = new GameObject("Flock Agent " + i);
            agentGO.transform.parent = transform;
            agentGO.transform.localPosition = Random.insideUnitCircle * 5f;

            agentGO.AddComponent<CircleCollider2D>();
            agentGO.layer = LayerMask.NameToLayer("Agent");

            var agent = agentGO.AddComponent<AutonomousAgent2D>();
            agent.maxSpeed = 5f;
            agent.maxForce = 10f;

            Vector2 toCenter = transform.Position2D() - agent.transform.Position2D();
            agent.rigid.AddForce(toCenter.normalized * 50f);

            var fl = agentGO.AddComponent<Flocking2D>();
            fl.sensorRadius = 3f;
            _flockBehaviors.Add(fl);

            fl.SetRadii(1f, 2f, 3f);

            if (useSamplePath) {
                var followPath = agent.AddSteering<FollowPath2D>();
                followPath.path = new Path2D(waypoints);
                followPath.path.bLoop = true;
                followPath.path.nearWaypointThresh = 3.5f;
                followPath.steerScaling = 0.5f;
            }

            else {
                var wander = agent.AddSteering<Wander2D>();
                wander.wanderDistance = 1.5f;
                wander.steerScaling = 0.3f;
                wander.wanderRadius = 0.5f;
                wander.wanderChangeRate = 1f;
            }
        }

        private void updateFlockingParameters()
        {
            foreach (Flocking2D f in _flockBehaviors) {
                f.SetScales(_separateScale, _alignScale, _cohesionScale);
            }
        }

        void OnValidate()
        {
            if (_flockBehaviors.Count > 0) {
                updateFlockingParameters();
            }
        }
    }
}
