using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutonomousAgents
{
    /// <summary>
    /// Warps game objects around the boundries if they go outside the area.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class AreaWarp2D : MonoBehaviour
    {

        private BoxCollider2D _area;
        private enum Side { TOP, BOTTOM, RIGHT, LEFT };

        void Awake()
        {
            _area = GetComponent<BoxCollider2D>();
            _area.isTrigger = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (outsideTop(other)) {
                teleport(other, Side.BOTTOM);
            }

            else if (outsideBottom(other)) {
                teleport(other, Side.TOP);
            }

            if (outsideRight(other)) {
                teleport(other, Side.LEFT);
            }

            else if (outsideLeft(other)) {
                teleport(other, Side.RIGHT);
            }
        }

        private bool outsideTop(Collider2D other)
        {
            return other.transform.position.y > Top;
        }

        private bool outsideBottom(Collider2D other)
        {
            return other.transform.position.y < Bottom;
        }

        private bool outsideRight(Collider2D other)
        {
            return other.transform.position.x > Right;
        }

        private bool outsideLeft(Collider2D other)
        {
            return other.transform.position.x < Left;
        }

        // Teleport the game object to the specified side.
        private void teleport(Collider2D other, Side side)
        {
            Vector2 newPosition = other.transform.position;

            switch (side) {
                case Side.TOP: newPosition.y = Top; break;
                case Side.BOTTOM: newPosition.y = Bottom; break;
                case Side.RIGHT: newPosition.x = Right; break;
                case Side.LEFT: newPosition.x = Left; break;
            }

            other.transform.position = newPosition;
        }

        public float Top
        {
            get { return _area.bounds.center.y + _area.bounds.extents.y; }
        }

        public float Bottom
        {
            get { return _area.bounds.center.y - _area.bounds.extents.y; }
        }

        public float Right
        {
            get { return _area.bounds.center.x + _area.bounds.extents.x; }
        }

        public float Left
        {
            get { return _area.bounds.center.x - _area.bounds.extents.x; }
        }
    }
}