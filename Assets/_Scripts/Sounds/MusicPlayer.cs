using System.Collections;
using System.Security.Cryptography;
using _Scripts.Prefs;
using UnityEngine;

namespace _Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _followUp;

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
        
        private AudioSource _audioSource;
        private AudioSource AudioSource => _audioSource == null ? _audioSource = GetComponent<AudioSource>() : _audioSource;
        
        public void Start()
        {
            GameEvents.GameEvents.OnMusicToggled += OnMusicToggle;
            AudioSource.mute = !PlayerPrefsService.Music;

            if (_followUp != null)
            {
                AudioSource.loop = false;
                StartCoroutine(SwapTracks(AudioSource.clip.length));
            }
        }

        private IEnumerator SwapTracks(float length)
        {
            yield return new WaitForSeconds(length);
            AudioSource.clip = _followUp;
            AudioSource.loop = true;
            AudioSource.Play();
        }
        
        private void OnDestroy()
        {
            GameEvents.GameEvents.OnMusicToggled -= OnMusicToggle;
            if (this == _instance)
            {
                _instance = null;
            }
        }

        private void OnMusicToggle(bool state)
        {
            _audioSource.mute = !state;
        }
    }
}