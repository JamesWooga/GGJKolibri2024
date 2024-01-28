using System;
using _Scripts.Objects;

namespace _Scripts.GameEvents
{
    // Hacky but Game Jam, fuck it :shrug:
    public static class GameEvents
    {
        public static event Action<(float height, float total)> OnObstacleCaught;
        public static event Action OnCameraZoomUpdated;
        public static event Action<DroppedObject> OnObstacleHitRope;
        public static event Action<bool> OnMusicToggled;
        public static event Action<bool> OnSoundToggled;

        public static void ObstacleCaught(float obstacleHeight, float totalWeight)
        {
            OnObstacleCaught?.Invoke((obstacleHeight, totalWeight));
        }

        public static void CameraZoomUpdated()
        {
            OnCameraZoomUpdated?.Invoke();
        }

        public static void ObstacleHitRope(DroppedObject droppedObject)
        {
            OnObstacleHitRope?.Invoke(droppedObject);
        }

        public static void MusicToggled(bool state)
        {
            OnMusicToggled?.Invoke(state);
        }
        
        public static void SoundToggled(bool state)
        {
            OnSoundToggled?.Invoke(state);
        }
    }
}