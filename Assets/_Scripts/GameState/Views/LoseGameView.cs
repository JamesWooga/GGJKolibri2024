using UnityEngine;

namespace _Scripts.GameState.Views
{
    public class LoseGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;

        private void Awake()
        {
            _root.SetActive(false);
            GameStateManager.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void OnDestroy()
        {
            GameStateManager.OnGameStateUpdated -= HandleGameStateUpdated;
        }

        private void HandleGameStateUpdated(GameState obj)
        {
            _root.SetActive(obj == GameState.Defeat);
        }
    }
}