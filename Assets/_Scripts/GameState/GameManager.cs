using System;
using UnityEngine;

namespace _Scripts.GameState
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    var obj = new GameObject("Game Manager");
                    _instance = obj.AddComponent<GameManager>();
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