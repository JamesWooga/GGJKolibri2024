using System;
using _Scripts.Objects;

namespace _Scripts.GameEvents
{
    // Hacky but Game Jam, fuck it :shrug:
    public static class GameEvents
    {
        public static event Action<float> OnObstacleCaught;
        public static event Action OnCameraZoomUpdated;
        public static event Action<DroppedObject> OnObstacleHitRope;

        public static void ObstacleCaught(float obstacleHeight)
        {
            OnObstacleCaught?.Invoke(obstacleHeight);
        }

        public static void CameraZoomUpdated()
        {
            OnCameraZoomUpdated?.Invoke();
        }

        public static void ObstacleHitRope(DroppedObject droppedObject)
        {
            OnObstacleHitRope?.Invoke(droppedObject);
        }
    }
}