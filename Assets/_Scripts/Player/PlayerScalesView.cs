using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Player
{
    public class PlayerScalesView : MonoBehaviour
    {
        [SerializeField] private bool _left;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _sliderDuration;
        
        private bool _initialized;
        private PlayerCatchPoint[] _points;
        private PlayerCatchPoint _point;

        private Tween _tween;
        
        private void Start()
        {
            _points = FindObjectsOfType<PlayerCatchPoint>();
            _point = _points.First(e => e.Left == _left);
            _initialized = true;
        }

        private void Update()
        {
            if (_initialized)
            {
                var sum = _points.Sum(e => e.TotalWeight);
                if (sum > 0)
                {
                    var percentage = _point.TotalWeight / sum;
                    _tween?.Kill();
                    _tween = _slider.DOValue(percentage, _sliderDuration);
                }
                
                _text.text = $"{_point.TotalWeight}kg";
            }
        }
    }
}