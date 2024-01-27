using System;

namespace _Scripts.GameEvents
{
    // Hacky but Game Jam, fuck it :shrug:
    public static class GameEvents
    {
        public static event Action<float> OnObstacleCaught;
        public static event Action OnCameraZoomUpdated;

        public static void ObstacleCaught(float obstacleHeight)
        {
            OnObstacleCaught?.Invoke(obstacleHeight);
        }

        public static void CameraZoomUpdated()
        {
            OnCameraZoomUpdated?.Invoke();
        }
    }
}