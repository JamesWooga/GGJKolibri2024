using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADbuttons : MonoBehaviour
{
    [SerializeField] Image buttonSprite_a;
    [SerializeField] Image buttonSprite_d;
    [SerializeField] Sprite buttonSprite_pressed;
    [SerializeField] Sprite buttonSprite_unpressed;

    [SerializeField] float interval;
    bool isA;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interval -= Time.deltaTime;

        if (interval <= 0)
        {
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
}
