using _Scripts.GameState;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Menu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _fadeSeconds;
        [SerializeField] private Ease _fadeOutEase;

        private void Start()
        {
            GameStateManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            var isPressingLeft = Input.GetKey(KeyCode.A);
            var isPressingRight = Input.GetKey(KeyCode.D);
            var isPressingUp = Input.GetKey(KeyCode.W);
            var isPressingDown = Input.GetKey(KeyCode.S);

            if ((isPressingLeft || isPressingRight || isPressingUp || isPressingDown) && GameStateManager.Instance.GameState == GameState.GameState.Menu)
            {
                GameStateManager.Instance.SetGameState(GameState.GameState.Play);
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
                .OnComplete(() => _root.SetActive(false));
        }

        private void FadeIn()
        {
            _canvasGroup.DOFade(1f, _fadeSeconds)
                .SetEase(_fadeOutEase);
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                _root.SetActive(true);
                FadeOut();
            }
        }
    }
}