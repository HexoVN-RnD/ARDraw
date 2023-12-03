using System.Collections;
using HSVPicker;
using UnityEngine;

public class ColorPickerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject colorPickerPanel;
    [SerializeField]
    private ColorPicker colorPicker;
    [SerializeField]
    private LineSettings lineSettings = null;
    [SerializeField]
    private float fadeDuration = 0.25f;
    private bool isFading = false;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = colorPickerPanel.GetComponent<CanvasGroup>();
    }
    
    public void OnColorPickerButtonClick()
    {
        if (isFading) return;
        colorPicker.CurrentColor = lineSettings.startColor;
        StartCoroutine(ShowPanel());
    }

    public void OnOutsidePanelClick()
    {
        if (isFading) return;
        SetColor(colorPicker.CurrentColor);
        StartCoroutine(HidePanel());
    }

    private IEnumerator ShowPanel()
    {
        colorPickerPanel.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator HidePanel()
    {
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = 1 - t;
            yield return null;
        }
        canvasGroup.alpha = 0;
        colorPickerPanel.gameObject.SetActive(false);
    }

    private void SetColor(Color color)
    {
        ARDebugManager.Instance.LogInfo($"Color selected: {color}");
        lineSettings.startColor = color;
        lineSettings.endColor = color;
    }
}
