using System.Collections;
using _Scripts.Prefs;
using UnityEngine;

namespace _Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private bool _isMusic;
        [SerializeField] private AudioClip _followUp;

        private AudioSource _audioSource;
        private AudioSource AudioSource => _audioSource == null ? _audioSource = GetComponent<AudioSource>() : _audioSource;
        
        private void Start()
        {
            GameEvents.GameEvents.OnMusicToggled += OnMusicToggle;
            GameEvents.GameEvents.OnSoundToggled += OnSoundToggle;

            if (_isMusic)
            {
                AudioSource.mute = !PlayerPrefsService.Music;
            }
            else
            {
                AudioSource.mute = !PlayerPrefsService.Sound;
            }

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
            GameEvents.GameEvents.OnSoundToggled -= OnSoundToggle;
        }

        private void OnMusicToggle(bool state)
        {
            if (_isMusic)
            {
                _audioSource.mute = !state;
            }
        }
        
        private void OnSoundToggle(bool state)
        {
            if (!_isMusic)
            {
                _audioSource.mute = !state;    
            }
        }
    }
}