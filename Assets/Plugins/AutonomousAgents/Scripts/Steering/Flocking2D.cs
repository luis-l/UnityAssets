
using UnityEngine;

namespace AutonomousAgents
{

    /// <summary>
    /// A steering behavior composed of separation, alignment, and cohesion.
    /// </summary>
    [DisallowMultipleComponent]
    public class Flocking2D : GroupSteering2D
    {

        private float _sepScale = 1f;
        private float _aliScale = 1f;
        private float _cohScale = 1f;

        private float _sepRadius = 1f;
        private float _aliRadius = 1f;
        private float _cohRadius = 1f;

        public override Vector2 SteerForce()
        {
            Vector2 total = Vector2.zero;

            int count = GetNeighbors();

            total += Separation2D.SteerForce(Agent, _agentColliders, count, _sepRadius) * _sepScale;
            total = Vector2.ClampMagnitude(total, Agent.maxForce);

            total += Alignment2D.SteerForce(Agent, _agentColliders, count, _aliRadius) * _aliScale;
            total = Vector2.ClampMagnitude(total, Agent.maxForce);

            total += Cohesion2D.SteerForce(Agent, _agentColliders, count, _cohRadius) * _cohScale;
            total = Vector2.ClampMagnitude(total, Agent.maxForce);

            return total;
        }

        /// <summary>
        /// Sets the steering scales.
        /// </summary>
        /// <param name="sep">Separation</param>
        /// <param name="ali">Alignment</param>
        /// <param name="coh">Cohesion</param>
        public void SetScales(float sep, float ali, float coh)
        {
            _sepScale = sep;
            _aliScale = ali;
            _cohScale = coh;
        }

        public void SetRadii(float sep, float ali, float coh)
        {
            _sepRadius = sep;
            _aliRadius = ali;
            _cohRadius = coh;
        }

        void OnDrawGizmosSelected()
        {
            Vector2 center = Agent.transform.position;
            DrawUtil.DrawCircle(center, _sepRadius, Color.red);
            DrawUtil.DrawCircle(center, _aliRadius, Color.magenta);
            DrawUtil.DrawCircle(center, _cohRadius, Color.yellow);
        }
    }
}