using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject counterDigitP;

    [SerializeField]
    private CounterDirection counterDirection;

    private List<CounterDigit> digitObjects = new List<CounterDigit>();

    private int counterNumber;

    public Color CounterDigitColor
    {
        get;
        set;
    } = Color.white;

    public int CounterNumber => counterNumber;

    private const float DIGIT_ANCHOR_PADDING_LR = 0.025f;

    public void SetCounterNumber(int counterNumber)
    {
        this.counterNumber = counterNumber;
        UpdateCounterDigits();
    }

    public void AssureListLength()
    {
        int digitCount = MathUtil.GetDigitCount(CounterNumber);

        // First, try to add all needed digits
        for(int i = 0; i < digitCount; ++i)
        {
            bool alreadyHasDigitObj = i < digitObjects.Count;

            // Create digit object if one does not exist
            if(!alreadyHasDigitObj)
            {
                GameObject counterDigitObj = Instantiate(counterDigitP);
                

                counterDigitObj.transform.SetParent(transform, false);

                CounterDigit counterDigit = counterDigitObj.GetComponent<CounterDigit>();
                counterDigit.SetDigitColor(CounterDigitColor);

                digitObjects.Add(counterDigit);
            }
        }

        bool hasExceedingObjects = digitObjects.Count > digitCount;

        if(!hasExceedingObjects)
        {
            return;
        }

        // Start at first digit after digit count
        // I.e. we have 7 digits, the number is 342 (3 digits)
        // i will start at 3, the 4th digit
        for(int i = digitCount; i < digitObjects.Count; ++i)
        {
            CounterDigit digitToTrim = digitObjects[i];
            Destroy(digitToTrim.gameObject);
        }


        // 7 digits, 342 (3 digits)
        // 4 to remove from index 3, (digits 4, 5, 6, 7)
        int countToRemove = digitObjects.Count - digitCount;

        digitObjects.RemoveRange(digitCount, countToRemove);
    }    

    public void UpdateCounterDigits()
    {
        AssureListLength();

        int i = 0;

        float oneWidth = 1f / digitObjects.Count;

        foreach(int digit in MathUtil.IterateDigitsLeftToRight(CounterNumber))
        {
            CounterDigit digitObject = digitObjects[i];
            digitObject.gameObject.name = $"{nameof(CounterDigit)}_{i}";

            Vector3 multiplierVector = counterDirection.GetDirectionVector();

            RectTransform digitTransform = digitObject.GetComponent<RectTransform>();

            float anchorMin = (oneWidth * i) + DIGIT_ANCHOR_PADDING_LR;
            float anchorMax = (oneWidth * (i + 1)) - DIGIT_ANCHOR_PADDING_LR;

            digitTransform.anchorMin = new Vector2(anchorMin, 0f);
            digitTransform.anchorMax = new Vector2(anchorMax, 1f);

            digitObject.SetDigit(digit);
            ++i;
        }

    }


}
