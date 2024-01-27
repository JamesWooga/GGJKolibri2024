using System;

namespace _Scripts.GameState
{
    public static class GameStateManager
    {
        public static GameState GameState { get; private set; }
        public static float Score { get; private set; }

        public static event Action<GameState> OnGameStateUpdated;
        
        public static void SetGameState(GameState gameState)
        {
            GameState = gameState;
            OnGameStateUpdated?.Invoke(gameState);
        }

        public static void SetScore(float score)
        {
            Score = score;
        }
    }
}