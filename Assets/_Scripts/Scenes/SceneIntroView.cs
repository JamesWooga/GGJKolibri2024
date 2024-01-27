using _Scripts.GameState;
using _Scripts.Menu;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace _Scripts.Scenes
{
    public class SceneIntroView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _startY;
        [SerializeField] private float _endY;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        [SerializeField] private Image _glowImage;
        [SerializeField] private float _lightUpDuration;
        [SerializeField] private float _finalAlpha;
        [SerializeField] private Ease _glowEase;
        [SerializeField] private MainMenuView _mainMenu;

        private void Start()
        {
            if (Time.time > 1)
            {
                GameManager.Instance.IsInputBlocked = false;
                return;
            }

            GameManager.Instance.IsInputBlocked = true;
            _rectTransform.anchoredPosition = new Vector2(0, _startY);
            _glowImage.SetAlpha(0f);
            _mainMenu.CanvasGroup.alpha = 0f;

            _rectTransform.DOLocalMoveY(_endY, _duration).SetEase(_ease)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            Invoke(nameof(DoFade), _duration);
        }

        private void DoFade()
        {
            _glowImage.DOFade(_finalAlpha, _lightUpDuration).SetEase(_glowEase)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            _mainMenu.CanvasGroup.DOFade(1.0f, _lightUpDuration)
                .OnComplete(() => GameManager.Instance.IsInputBlocked = false)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
    }
}