using _Scripts.Saving;
using TMPro;
using UnityEngine;

namespace _Scripts.GameState.Views
{
    public class LoseGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _highScoreText;
        
        private SaveFile _save;
        
        private void Awake()
        {
            _save = SaveSystem.GetSaveFile();
            _root.SetActive(false);
            GameStateManager.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void OnDestroy()
        {
            GameStateManager.OnGameStateUpdated -= HandleGameStateUpdated;
        }

        private void HandleGameStateUpdated(GameState obj)
        {
            if (GameStateManager.Score > _save.Highscore)
            {
                _save.Highscore = GameStateManager.Score;
            }

            _scoreText.text = GameStateManager.Score.ToString("F0");
            _highScoreText.text = _save.Highscore.ToString("F0");
            _root.SetActive(obj == GameState.Defeat);
            SaveSystem.Save(_save);
        }
    }
}