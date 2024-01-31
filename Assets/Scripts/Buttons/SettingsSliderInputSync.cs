using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSliderInputSync : MonoBehaviour
{
    private TMP_InputField brushSizeInputField;
    private Slider brushSizeSlider;
    private bool isUpdate;
    [SerializeField] private float baseValue = 0.005f;

    private void Awake()
    {
        brushSizeInputField = GetComponentInChildren<TMP_InputField>();
        brushSizeSlider = GetComponentInChildren<Slider>();
        brushSizeSlider.onValueChanged.AddListener(SliderValueChanged);
        brushSizeInputField.onEndEdit.AddListener(InputFieldValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        if (isUpdate) return;

        isUpdate = true;
        value = baseValue * value;
        brushSizeInputField.text = value.ToString();
        isUpdate = false;
    }

    private void InputFieldValueChanged(string stringValue)
    {
        if (isUpdate) return;

        if (float.TryParse(stringValue, out float value))
        {
            isUpdate = true;
            value /= baseValue;
            value = Mathf.Clamp(value, brushSizeSlider.minValue, brushSizeSlider.maxValue);
            brushSizeSlider.value = value;
            isUpdate = false;
        }
    }
}
