using System;
using UnityEngine;

namespace _Scripts.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager _instance;

        public static GameStateManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    var obj = new GameObject("Game Manager");
                    _instance = obj.AddComponent<GameStateManager>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
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