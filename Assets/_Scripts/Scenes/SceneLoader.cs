using _Scripts.GameState;
using _Scripts.Sounds;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Scripts.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private InputActionReference _restart;

        private float _time = 99999999f;
        private bool _hasRestarted;

        private void Start()
        {
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

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
            if (!_hasRestarted && Time.time > _time && GameManager.Instance.GameState == GameState.GameState.GameOver)
            {
                bool restart = false;
#if UNITY_ANDROID && !UNITY_EDITOR
                restart = Input.touches.Length > 0;
#else
                restart = _restart.action.IsPressed();
#endif
                if (restart)
                {
                    _hasRestarted = true;
                    var activeScene = SceneManager.GetActiveScene();
                    MusicPlayer.Instance.Restart();
                    SceneManager.LoadScene(activeScene.name);
                    SoundsPlayer.Instance.Unmute();
                }
            }
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.GameOver)
            {
                _time = Time.time + _delay;
            }
        }
    }
}