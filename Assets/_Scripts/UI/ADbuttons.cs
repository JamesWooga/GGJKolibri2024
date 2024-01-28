using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class ADbuttons : MonoBehaviour
{
    [SerializeField] private Image buttonSprite_a;
    [SerializeField] private Image buttonSprite_d;
    [SerializeField] private Sprite buttonSprite_pressed;
    [SerializeField] private Sprite buttonSprite_unpressed;
    [SerializeField] private TMP_Text _leftKeyText;
    [SerializeField] private TMP_Text _rightKeyText;

    [SerializeField] private float interval;
    private bool isA;

    private float _nextTime;
    private string _currentDevice = "keyboard";

    private void OnEnable()
    {
        if (Time.time > 0)
        {
            Init();
        }
        else
        {
            Invoke(nameof(Init), 0.2f);    
        }
        
        // Subscribe to the input event
        UpdateText();
    }

    private void Init()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        // Unsubscribe from the input event
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device.displayName != "Wireless Controller" && device.displayName != "Mouse")
        {
            if (device.displayName == "Keyboard")
            {
                _currentDevice = "keyboard";
            }
            else
            {
                _currentDevice = "controller";
            }
        }
        UpdateText();
    }

    private void Update()
    {
        if (Time.time < _nextTime)
        {
            return;
        }
        _nextTime = Time.time + interval;

        if (isA)
        {
            buttonSprite_a.sprite = buttonSprite_unpressed;
            buttonSprite_d.sprite = buttonSprite_pressed;
            isA = false;
        } else
        {
            buttonSprite_a.sprite = buttonSprite_pressed;
            buttonSprite_d.sprite = buttonSprite_unpressed;
            isA = true;
        }
    }

    private void UpdateText()
    {
        _leftKeyText.text = _currentDevice == "keyboard" ? "A" : "←";
        _rightKeyText.text = _currentDevice == "keyboard" ? "D" : "→";
    }
}
