using _Scripts.GameState;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Player
{
    public class PlayerInputView : MonoBehaviour
    {
        [SerializeField] private Image _leftButton;
        [SerializeField] private Image _rightButton;
        [SerializeField] private Color _buttonPressedColor;
        [SerializeField] private Color _buttonReleasedColor;
        
        private void FixedUpdate()
        {
            if (GameManager.Instance.IsInputBlocked)
            {
                return;
            }
            
            var isPressingLeft = Input.GetKey(KeyCode.A);
            var isPressingRight = Input.GetKey(KeyCode.D);

            _leftButton.color = isPressingLeft ? _buttonPressedColor : _buttonReleasedColor;
            _rightButton.color = isPressingRight ? _buttonPressedColor : _buttonReleasedColor;
        }
    }
}