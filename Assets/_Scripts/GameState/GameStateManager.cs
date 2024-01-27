using System;
using UnityEngine;

namespace _Scripts.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager _instance;

        public static GameStateManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }
        
        public GameState GameState { get; private set; } = GameState.Menu;

        public float Score { get; private set; }

        public event Action<GameState> OnGameStateUpdated;
        
        public void SetGameState(GameState gameState)
        {
            GameState = gameState;
            OnGameStateUpdated?.Invoke(gameState);
        }

        public void SetScore(float score)
        {
            Score = score;
        }
    }
}