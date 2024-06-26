﻿using System;
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

        public bool IsInputBlocked { get; set; } = false;
        
        public void SetGameState(GameState gameState)
        {
            GameState = gameState;
            OnGameStateUpdated?.Invoke(gameState);
        }

        public void SetScore(float score)
        {
            Score = score;
        }

        public float CurrentTime { get; private set; }
        private bool _count;
        
        public void StartRun()
        {
            _count = true;
            CurrentTime = 0;
        }

        public void EndRun()
        {
            _count = false;
        }

        public void Update()
        {
            if (_count)
            {
                CurrentTime += Time.deltaTime;    
            }
        }
    }
}