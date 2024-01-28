using System;
using _Scripts.Player;
using _Scripts.Saving;
using _Scripts.Sounds;
using DG.Tweening;
using TMPro;
using UnityEngine;

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
        
        private SaveFile _save;
        private bool _isLost;
        
        private void Start()
        {
            _save = SaveSystem.GetSaveFile();
            _root.SetActive(false);
            GameManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
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

                _canvasGroup.DOFade(1f, 0.5f);
                SaveSystem.Save(_save);
            }
        }
    }
}