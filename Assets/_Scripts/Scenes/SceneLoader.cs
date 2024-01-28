using _Scripts.GameState;
using _Scripts.Sounds;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Scripts.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private InputActionReference _restart;
        
        private void OnEnable()
        {
            _restart.action.Enable();
        }

        private void OnDisable()
        {
            _restart.action.Disable();
        }
        
        private void Update()
        {
            if (_restart.action.IsPressed() && !GameManager.Instance.IsInputBlocked)
            {
                var activeScene = SceneManager.GetActiveScene();
                MusicPlayer.Instance.Restart();
                SceneManager.LoadScene(activeScene.name);
                SoundsPlayer.Instance.Unmute();
            }
        }
    }
}