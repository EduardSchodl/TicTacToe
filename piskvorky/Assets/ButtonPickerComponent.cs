using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPickerComponent : MonoBehaviour
{
    [SerializeField]
    private Button lowerButton;
    [SerializeField]
    private Button higherButton;

    [SerializeField]
    private TextMeshProUGUI pickerText;

    private int _iterator = 0;

    public int Iterator => _iterator;

    private IPickerExtValueIntoIndex extUpdater;

    public void SetIteratorValue(int newIteratorvalue)
    {
        int lastIteratorvalue = _iterator;
        _iterator = ClampValueToPicker(newIteratorvalue);

        bool valueChanged = _iterator != lastIteratorvalue;
        if (valueChanged)
        {
            UpdatePickerText();
        }
    }

    public int ClampValueToPicker(int value)
    {
        return MathUtil.IntClamp(value, MinValue, MaxValue);
    }

    private IPickerIndexToTextTransformer indexTransformer;

    private int _minvalue = int.MinValue;
    public int MinValue
    {
        get => _minvalue;
        set
        {
            _minvalue = value;
            // Set iterator to recheck clamping the value to the new min/max value range
            SetIteratorValue(Iterator);
        }
    }

    private int _maxValue = int.MaxValue;
    public int MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            // Set iterator to recheck clamping the value to the new min/max value range
            SetIteratorValue(Iterator);
        }
    }

    private void Start()
    {
        lowerButton.onClick.AddListener(LowerButtonPressed);
        higherButton.onClick.AddListener(HigherButtonPressed);

        TryGetComponent(out indexTransformer);
        TryGetComponent(out extUpdater);

        UpdatePickerText();
    }

    private void OnEnable()
    {
        TryUpdateComponentFromExtValue();
    }

    private void TryUpdateComponentFromExtValue()
    {
        if (extUpdater == null)
        {
            return;
        }

        int indexFromExternal = extUpdater.GetPickerIndexFromExtValue();
        SetIteratorValue(indexFromExternal);
    }

    public void LowerButtonPressed()
    {
        SetIteratorValue(Iterator - 1);
    }

    public void HigherButtonPressed()
    {
        SetIteratorValue(Iterator + 1);
    }

    public void UpdatePickerText()
    {
        pickerText.text = GetPickerTextFromIndex();
    }

    public string GetPickerTextFromIndex()
    {
        // By default just use the index
        if(indexTransformer == null)
        {
            return Iterator.ToString();
        }

        return indexTransformer.GetTextFromPickerIndex(Iterator);
    }
}
