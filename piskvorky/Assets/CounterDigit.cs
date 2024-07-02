using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CounterDigit : MonoBehaviour
{
    private Image imageComponent;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    public void SetDigitColor(Color color)
    {
        imageComponent.color = color;
    }    

    private int digit;
    public int Digit => digit;

    public void SetDigit(int digit)
    {
        this.digit = digit;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        imageComponent.sprite = ResourcesManager.INSTANCE.GetDigitSpriteByNumber(digit);
    }


}
