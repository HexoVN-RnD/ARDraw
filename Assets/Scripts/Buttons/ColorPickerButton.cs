using System.Collections;
using System.Collections.Generic;
using HSVPicker;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPickerButton : MonoBehaviour
{
    [SerializeField] private GameObject colorPickerPanel;
    [SerializeField] private GameObject innerPanel;
    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private LineSettings lineSettings = null;
    [SerializeField] private float fadeDuration = 0.25f;
    private ARDrawManager arDrawManager;
    private bool isFading = false;

    private CanvasGroup colorPickerCanvasGroup;

    private void Awake()
    {
        colorPickerCanvasGroup = colorPickerPanel.GetComponent<CanvasGroup>();
        arDrawManager = FindObjectOfType<ARDrawManager>();
    }

    public void OnColorPickerButtonClick()
    {
        if (isFading) return;
        colorPicker.CurrentColor = lineSettings.startColor;
        arDrawManager.AllowDraw(false);
        StartCoroutine(FadePanel(true));
    }

    public void CheckOutsidePanelClick()
    {
        if (isFading) return;

        // Set up the PointerEventData with the current mouse position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the Graphics Raycaster and mouse click position
        EventSystem.current.RaycastAll(eventData, results);

        // For every result returned, check if the GameObject is the inner panel
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == innerPanel)
            {
                // Click was on the inner panel, do nothing
                return;
            }
        }
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
            arDrawManager.AllowDraw(true);
        }
    }

    private void SetColor(Color color)
    {
        ARDebugManager.Instance.LogInfo($"Color selected: {color}");
        lineSettings.startColor = color;
        lineSettings.endColor = color;
    }
}
