using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ADbuttons : MonoBehaviour
{
    [SerializeField] private Image buttonSprite_a;
    [SerializeField] private Image buttonSprite_d;
    [SerializeField] private Sprite buttonSprite_pressed;
    [SerializeField] private Sprite buttonSprite_unpressed;

    [SerializeField] private float interval;
    private bool isA;

    private float _nextTime;
    
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
}
