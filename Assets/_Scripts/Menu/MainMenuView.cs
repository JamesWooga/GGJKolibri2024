using _Scripts.GameState;
using _Scripts.Prefs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Scripts.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _gameCanvasGroup;
        [SerializeField] private float _fadeSeconds;
        [SerializeField] private Ease _fadeOutEase;
        [SerializeField] private Button _toggleMusicButton;
        [SerializeField] private Button _toggleSoundButton;
        [SerializeField] private GameObject _musicMutedRoot;
        [SerializeField] private GameObject _soundMutedRoot;
        [SerializeField] private InputActionReference _tiltLeft;
        [SerializeField] private InputActionReference _tiltRight;

        public CanvasGroup CanvasGroup => _canvasGroup;

        private void Start()
        {
            _gameCanvasGroup.alpha = 0f;
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
            
            _toggleMusicButton.onClick.AddListener(ToggleMusic);
            _toggleSoundButton.onClick.AddListener(ToggleSound);
            
            _musicMutedRoot.SetActive(!PlayerPrefsService.Music);
            _soundMutedRoot.SetActive(!PlayerPrefsService.Sound);
        }

        private void OnDestroy()
        {
            _toggleMusicButton.onClick.RemoveAllListeners();
            _toggleSoundButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            if (GameManager.Instance.IsInputBlocked)
            {
                return;
            }

            var isPressingLeft = _tiltLeft.action.IsPressed();
            var isPressingRight = _tiltRight.action.IsPressed();

            if ((isPressingLeft || isPressingRight) && GameManager.Instance.GameState == GameState.GameState.Menu)
            {
                GameManager.Instance.SetGameState(GameState.GameState.Play);
                GameManager.Instance.StartRun();
            }
            
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void FadeOut()
        {
            _canvasGroup.DOFade(0f, _fadeSeconds)
                .SetEase(_fadeOutEase)
                .OnComplete(() => _root.SetActive(false))
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            _gameCanvasGroup.DOFade(1f, _fadeSeconds)
                .SetEase(_fadeOutEase)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                _root.SetActive(true);
                FadeOut();
            }
        }

        private void ToggleMusic()
        {
            PlayerPrefsService.Music = !PlayerPrefsService.Music;
            _musicMutedRoot.SetActive(!PlayerPrefsService.Music);
        }

        private void ToggleSound()
        {
            PlayerPrefsService.Sound = !PlayerPrefsService.Sound;
            _soundMutedRoot.SetActive(!PlayerPrefsService.Sound);
        }
    }
}