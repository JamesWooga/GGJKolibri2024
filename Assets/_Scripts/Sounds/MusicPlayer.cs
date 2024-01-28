using System.Collections;
using _Scripts.Prefs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _followUp;
        [SerializeField] private float _totalWeightToSpeedUpRatio;

        private static MusicPlayer _instance;

        public static MusicPlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("Music Player");
                    _instance = obj.AddComponent<MusicPlayer>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private float _musicLowPassDuration;
        [SerializeField] private float _pitchChangeDuration;
        
        private AudioSource _audioSource;
        private AudioSource AudioSource => _audioSource == null ? _audioSource = GetComponent<AudioSource>() : _audioSource;

        private Coroutine _coroutine;
        private AudioClip _originalClip;
        private Tween _speedTween;
        
        public void Lost()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _audioMixerGroup.audioMixer.DOSetFloat("MusicLowPass", 400f, _musicLowPassDuration)
                .SetAutoKill(true);
            
            _audioMixerGroup.audioMixer.DOSetFloat("MusicPitch", 0f, _pitchChangeDuration)
                .SetAutoKill(true);
        }

        public void Restart()
        {
            AudioSource.clip = _originalClip;
            AudioSource.Play();
            
            if (_followUp != null)
            {
                AudioSource.loop = false;
                _coroutine = StartCoroutine(SwapTracks());
            }
            
            _audioMixerGroup.audioMixer.DOSetFloat("MusicPitch", 1f, _pitchChangeDuration)
                .SetAutoKill(true);
        }
        
        public void Start()
        {
            _originalClip = AudioSource.clip;
            GameEvents.GameEvents.OnMusicToggled += OnMusicToggle;
            GameEvents.GameEvents.OnObstacleCaught += HandleObstacleCaught;

            AudioSource.mute = !PlayerPrefsService.Music;

            Restart();
        }

        private IEnumerator SwapTracks()
        {
            while (AudioSource.isPlaying)
            {
                yield return null;
            }

            AudioSource.clip = _followUp;
            AudioSource.loop = true;
            AudioSource.Play();
        }
        
        private void OnDestroy()
        {
            GameEvents.GameEvents.OnMusicToggled -= OnMusicToggle;
            GameEvents.GameEvents.OnObstacleCaught -= HandleObstacleCaught;
            _speedTween?.Kill();

            if (this == _instance)
            {
                _instance = null;
            }
        }

        private void OnMusicToggle(bool state)
        {
            _audioSource.mute = !state;
        }
        
        private void HandleObstacleCaught((float height, float weight) a)
        {
            var speed = 1 + ((a.weight * _totalWeightToSpeedUpRatio) / 100f);
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(SwapTracks());
            
            _speedTween?.Kill();
            _speedTween = _audioMixerGroup.audioMixer.DOSetFloat("MusicPitch", speed, 0.25f);
        }
    }
}