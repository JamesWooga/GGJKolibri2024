using System;
using _Scripts.GameState;
using _Scripts.Sounds;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

namespace _Scripts.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        private IDisposable _listener;
        [SerializeField] private float _delay;

        private float _time;
        private bool _hasRestarted;
        
        private void OnEnable()
        {
            _listener = InputSystem.onAnyButtonPress.Call(TryRestart);
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void OnDisable()
        {
            _listener.Dispose();
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.GameOver)
            {
                _time = Time.time + _delay;    
            }
        }

        private void TryRestart(InputControl button)
        {
            if (!_hasRestarted && Time.time > _time && GameManager.Instance.GameState == GameState.GameState.GameOver && !GameManager.Instance.IsInputBlocked)
            {
                _hasRestarted = true;
                var activeScene = SceneManager.GetActiveScene();
                MusicPlayer.Instance.Restart();
                SceneManager.LoadScene(activeScene.name);
                SoundsPlayer.Instance.Unmute();
            }
        }
    }
}