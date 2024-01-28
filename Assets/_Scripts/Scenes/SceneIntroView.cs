using _Scripts.GameState;
using _Scripts.Menu;
using _Scripts.Sounds;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private float _originalLowPassFilter;
        [SerializeField] private float _finalLowPassFilter;
        [SerializeField] private float _musicDuration;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private AudioClip _audioClip;
        
        private void Start()
        {
            if (Time.time > 1)
            {
                _audioMixerGroup.audioMixer.DOSetFloat("MusicLowPass", _finalLowPassFilter, 0.5f);
                GameManager.Instance.IsInputBlocked = false;
                return;
            }

            SoundsPlayer.Instance.PlaySound(_audioClip);
            GameManager.Instance.IsInputBlocked = true;
            _rectTransform.anchoredPosition = new Vector2(0, _startY);
            _glowImage.SetAlpha(0f);
            _mainMenu.CanvasGroup.alpha = 0f;
            _audioMixerGroup.audioMixer.SetFloat("MusicLowPass", _originalLowPassFilter);

            _rectTransform.DOLocalMoveY(_endY, _duration).SetEase(_ease)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            
            _audioMixerGroup.audioMixer.DOSetFloat("MusicLowPass", _finalLowPassFilter, _musicDuration)
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