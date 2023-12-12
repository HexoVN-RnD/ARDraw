using System.Collections;
using HSVPicker;
using UnityEngine;

public class ColorPickerButton : MonoBehaviour
{
    [SerializeField] private GameObject colorPickerPanel;
    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private LineSettings lineSettings = null;
    [SerializeField] private float fadeDuration = 0.25f;
    private bool isFading = false;

    private CanvasGroup colorPickerCanvasGroup;

    private void Awake()
    {
        colorPickerCanvasGroup = colorPickerPanel.GetComponent<CanvasGroup>();
    }

    public void OnColorPickerButtonClick()
    {
        if (isFading) return;
        colorPicker.CurrentColor = lineSettings.startColor;
        StartCoroutine(FadePanel(true));
    }

    public void OnOutsidePanelClick()
    {
        if (isFading) return;
        SetColor(colorPicker.CurrentColor);
        StartCoroutine(FadePanel(false));
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
        colorPickerPanel.SetActive(true);
        float startTime = Time.time;
        float startAlpha = fadeIn ? 0 : 1;
        float endAlpha = fadeIn ? 1 : 0;

        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            colorPickerCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        colorPickerCanvasGroup.alpha = endAlpha;

        if (!fadeIn)
        {
            colorPickerPanel.SetActive(false);
        }
    }

    private void SetColor(Color color)
    {
        ARDebugManager.Instance.LogInfo($"Color selected: {color}");
        lineSettings.startColor = color;
        lineSettings.endColor = color;
    }
}
