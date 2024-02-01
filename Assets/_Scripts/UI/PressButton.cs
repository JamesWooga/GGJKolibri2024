using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class PressButton : MonoBehaviour
    {
        [SerializeField] private Image buttonSprite_a;
        [SerializeField] private Sprite buttonSprite_pressed;
        [SerializeField] private Sprite buttonSprite_unpressed;
        [SerializeField] private float interval;
        [SerializeField] private Transform _buttonTransform;
        [SerializeField] private float _startY;
        [SerializeField] private float _endY;

        private bool _on;
        private float _nextTime;
    
        private void Update()
        {
            if (Time.time < _nextTime)
            {
                return;
            }
            _nextTime = Time.time + interval;

            buttonSprite_a.sprite = _on ? buttonSprite_unpressed : buttonSprite_pressed;
            _on = !_on;

            var value = _on ? _endY : _startY;
            _buttonTransform.DOLocalMoveY(value, 0.01f);
        }
    }
}