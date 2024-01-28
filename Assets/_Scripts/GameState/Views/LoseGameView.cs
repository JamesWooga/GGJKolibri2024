using _Scripts.Saving;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.GameState.Views
{
    public class LoseGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private float _musicLowPassDuration;
        
        private SaveFile _save;
        
        private void Start()
        {
            _save = SaveSystem.GetSaveFile();
            _root.SetActive(false);
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void HandleGameStateUpdated(GameState obj)
        {
            if (GameManager.Instance.Score > _save.Highscore)
            {
                _save.Highscore = GameManager.Instance.Score;
            }
            
            _scoreText.text = GameManager.Instance.Score.ToString("F0");
            _highScoreText.text = _save.Highscore.ToString("F0");
            _root.SetActive(obj == GameState.GameOver);
            SaveSystem.Save(_save);

            if (obj == GameState.GameOver)
            {
                _audioMixerGroup.audioMixer.DOSetFloat("MusicLowPass", 400f, _musicLowPassDuration)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }
    }
}