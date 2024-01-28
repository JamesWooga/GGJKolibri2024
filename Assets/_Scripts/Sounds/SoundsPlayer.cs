using _Scripts.Objects;
using _Scripts.Prefs;
using UnityEngine;

namespace _Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundsPlayer : MonoBehaviour
    {
        private static SoundsPlayer _instance;

        public static SoundsPlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("Sound Player");
                    _instance = obj.AddComponent<SoundsPlayer>();
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

        [SerializeField] private SoundsConfig _soundsConfig;

        private AudioSource _audioSource;
        private AudioSource AudioSource => _audioSource == null ? _audioSource = GetComponent<AudioSource>() : _audioSource;
        
        public void PlaySound(DroppedObjectType objectType, SoundsConfig.SoundType soundType)
        {
            var clip = _soundsConfig.GetClip(objectType, soundType);
            AudioSource.PlayOneShot(clip);
        }
        
        public void Start()
        {
            GameEvents.GameEvents.OnSoundToggled += OnSoundToggle;
            AudioSource.mute = !PlayerPrefsService.Sound;
        }

        public void Mute()
        {
            _audioSource.mute = true;
        }

        public void Unmute()
        {
            _audioSource.mute = !PlayerPrefsService.Sound;
        }
        
        private void OnDestroy()
        {
            GameEvents.GameEvents.OnSoundToggled -= OnSoundToggle;
            if (this == _instance)
            {
                _instance = null;
            }
        }

        private void OnSoundToggle(bool state)
        {
            _audioSource.mute = !state;
        }
    }
}