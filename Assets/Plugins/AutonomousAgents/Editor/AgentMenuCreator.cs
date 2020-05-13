using UnityEngine;
using UnityEditor;

namespace AutonomousAgents
{
    /// <summary>
    /// Create a basic autonomous agent without an steering behaviors.
    /// </summary>
    public class AgentMenuCreator
    {
        private static Sprite _obstacleSprite;
        private static Sprite _wallSprite;

        private const string kOtherPath = "GameObject/Create Other/";
        private const string kAgentPath = kOtherPath + "Agent/";
        private const string kEnvPath = kOtherPath + "Environment/";

        static AgentMenuCreator()
        {
            _obstacleSprite = Resources.Load<Sprite>("Sprites/Circle");
            _wallSprite = Resources.Load<Sprite>("Sprites/Square");
        }

        [MenuItem(kAgentPath + "Empty Agent", false, 11)]
        static void CreateEmptyAgent()
        {
            new GameObject("Agent", typeof(AutonomousAgent2D));
        }

        [MenuItem(kAgentPath + "Obstacle Avoidance Agent", false, 12)]
        static void CreateObstacleAvoidanceAgent()
        {
            new GameObject("Agent: Avoid Obstacles", typeof(ObstacleAvoidance2D));
        }


        [MenuItem(kEnvPath + "Obstacle", false, 40)]
        static void CreateBasicObstacle()
        {
            var obs = new GameObject("Obstacle");
            var renderer = obs.AddComponent<SpriteRenderer>();
            renderer.sprite = _obstacleSprite;
            renderer.color = Color.gray;

            obs.AddComponent<CircleCollider2D>();
            obs.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        }

        [MenuItem(kEnvPath + "Wall", false, 41)]
        static void CreateBasicWall()
        {
            var wall = new GameObject("Wall");
            var renderer = wall.AddComponent<SpriteRenderer>();
            renderer.sprite = _wallSprite;
            renderer.color = Color.gray;

            wall.AddComponent<BoxCollider2D>();
            wall.transform.localScale = new Vector3(5f, 0.5f, 1f);
        }
    }
}