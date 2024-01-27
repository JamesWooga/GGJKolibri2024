using _Scripts.GameState;
using _Scripts.Objects;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerCatchPoint : MonoBehaviour
    {
        [SerializeField] private bool _left;
        [SerializeField] private Transform _root;
        [SerializeField] private TMP_Text _weightText;
        [SerializeField] private CanvasGroup _scoreRoot;
        [SerializeField] private float _fadeDuration;
        
        public bool Left => _left;
        public float TotalWeight { get; private set; }

        private void Start()
        {
            GameStateManager.Instance.OnGameStateUpdated += HandleGameStateUpdated;
        }

        private void HandleGameStateUpdated(GameState.GameState obj)
        {
            if (obj == GameState.GameState.Play)
            {
                _scoreRoot.DOFade(1f, _fadeDuration);
            }
            else
            {
                _scoreRoot.DOFade(0f, _fadeDuration);
            }
        }

        public void Attach(DroppedObject droppedObject)
        {
            droppedObject.Rigidbody2D.bodyType = RigidbodyType2D.Static;
            droppedObject.transform.SetParent(_root);
            Destroy(droppedObject.Rigidbody2D);

            droppedObject.SetTag(_left ? "DroppedObjectLeft" : "DroppedObjectRight");
            GameEvents.GameEvents.ObstacleCaught(droppedObject.transform.position.y);
            
            TotalWeight += droppedObject.Weight;
            _weightText.text = $"{TotalWeight}kg";
        }
    }
}