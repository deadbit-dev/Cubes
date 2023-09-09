using System;
using UnityEngine;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class SwipeController : Singleton<SwipeController>
    {
        private Vector2 downPosition;
        private Vector2 upPosition;

        public event Action<SwipeData> OnSwipe = delegate {};

        public void ProccesInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                upPosition = Input.mousePosition;
                downPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                downPosition = Input.mousePosition;
                DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                if (IsVerticalSwipe())
                {
                    var direction = downPosition.y - upPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = downPosition.x - upPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);
                }

                upPosition = downPosition;
            }
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > Config.MinDistanceForSwipe ||
                HorizontalMovementDistance() > Config.MinDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(downPosition.y - upPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(downPosition.x - upPosition.x);
        }

        private void SendSwipe(SwipeDirection direction)
        {
            var swipeData = new SwipeData()
            {
                Direction = direction,
                StartPosition = downPosition,
                EndPosition = upPosition
            };

            OnSwipe(swipeData);
        }
    }
}