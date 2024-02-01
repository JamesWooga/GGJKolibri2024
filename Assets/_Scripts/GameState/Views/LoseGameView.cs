using System;
using _Scripts.Player;
using _Scripts.Saving;
using _Scripts.Sounds;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace _Scripts.GameState.Views
{
    public class LoseGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _weightText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private GameObject _buttonParent;
        [SerializeField] private Transform _touch;
        
        private SaveFile _save;
        private bool _isLost;
        
        private string _currentDevice = "keyboard";
        
        private void Start()
        {
            _save = SaveSystem.GetSaveFile();
            _root.SetActive(false);
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
            
            DOTween.Sequence()
                .Append(_touch.DOScale(Vector3.one * 1.15f, 2.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            
#if UNITY_ANDROID && !UNITY_EDITOR
            _touch.gameObject.SetActive(true);
            _buttonParent.SetActive(false);
#else
            _touch.gameObject.SetActive(false);
            _buttonParent.SetActive(true);
#endif
        }
        
        private void OnEnable()
        {
            if (Time.time > 0)
            {
                Init();
            }
            else
            {
                Invoke(nameof(Init), 0.2f);    
            }
        
            // Subscribe to the input event
            UpdateText();
        }

        private void Init()
        {
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnDisable()
        {
            // Unsubscribe from the input event
            InputSystem.onEvent -= OnInputEvent;
        }
        
        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
#if UNITY_WEBGL
            UpdateText();
            return;
#endif
            if (device.displayName != "Wireless Controller" && device.displayName != "Mouse")
            {
                if (device.displayName == "Keyboard")
                {
                    _currentDevice = "keyboard";
                }
                else
                {
                    _currentDevice = "controller";
                }
            }
            UpdateText();
        }

        private void HandleGameStateUpdated(GameState obj)
        {
            _root.SetActive(obj == GameState.GameOver);

            if (obj == GameState.GameOver && !_isLost)
            {
                var finalScore = GameManager.Instance.Score + GameManager.Instance.CurrentTime;
                if (finalScore > _save.Highscore)
                {
                    _save.Highscore = GameManager.Instance.Score;
                }
            
                _scoreText.text = finalScore.ToString("F0");
                _highScoreText.text = _save.Highscore.ToString("F0");
                
                _isLost = true;
                _canvasGroup.alpha = 0f;
                var player = FindObjectOfType<PlayerController>();
                var ts = TimeSpan.FromSeconds(GameManager.Instance.CurrentTime);
                _weightText.text = player.AllWeight.ToString("F0");
                _timeText.text = $"{ts.Minutes:D2}:{ts.Seconds:D2}";
                MusicPlayer.Instance.Lost();
                SoundsPlayer.Instance.Mute();

                _canvasGroup.DOFade(1f, 0.5f)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
                SaveSystem.Save(_save);
            }
        }
        
        private void UpdateText()
        {
            _buttonText.text = _currentDevice == "keyboard" ? "R" : "A";
        }
    }
}