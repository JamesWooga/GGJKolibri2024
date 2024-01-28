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
        
        private void OnEnable()
        {
            _listener = InputSystem.onAnyButtonPress.Call(TryRestart);
        }

        private void OnDisable()
        {
            _listener.Dispose();
        }

        private void TryRestart(InputControl button)
        {
            if (GameManager.Instance.GameState == GameState.GameState.GameOver && !GameManager.Instance.IsInputBlocked)
            {
                var activeScene = SceneManager.GetActiveScene();
                MusicPlayer.Instance.Restart();
                SceneManager.LoadScene(activeScene.name);
                SoundsPlayer.Instance.Unmute();
            }
        }
    }
}