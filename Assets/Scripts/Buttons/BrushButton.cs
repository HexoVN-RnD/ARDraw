using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BrushButton : MonoBehaviour
{
    [SerializeField] private GameObject brushPanel;
    [SerializeField] private GameObject innerPanel;
    [SerializeField] private TMP_InputField brushSizeInputField;
    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private LineSettings lineSettings = null;
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float baseWidthValue = 0.005f;
    [SerializeField] private float baseTextureScaleX = 200f;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    private ARDrawManager arDrawManager;
    private CanvasGroup brushPanelCanvasGroup;
    private bool isFading = false;

    private void Awake()
    {
        brushSizeSlider.onValueChanged.AddListener(SliderValueChanged);
        brushSizeInputField.onValueChanged.AddListener(InputFieldValueChanged);
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        brushPanelCanvasGroup = brushPanel.GetComponent<CanvasGroup>();
        arDrawManager = FindObjectOfType<ARDrawManager>();
    }

    private void SliderValueChanged(float value)
    {
        brushSizeInputField.text = value.ToString();
    }

    private void InputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float inputValue))
        {
            inputValue = Mathf.Clamp(inputValue, brushSizeSlider.minValue, brushSizeSlider.maxValue);
            brushSizeSlider.value = inputValue;
            brushSizeInputField.text = inputValue.ToString();
        }
    }

    private void OnConfirmButtonClick()
    {
        lineSettings.startWidth = baseWidthValue * brushSizeSlider.value;
        lineSettings.endWidth = baseWidthValue * brushSizeSlider.value;
        lineSettings.textureScale.x = baseTextureScaleX/brushSizeSlider.value;
        StartCoroutine(FadePanel());
    }

    private void OnCancelButtonClick()
    {
        StartCoroutine(FadePanel());
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

        // Click was outside the inner panel, fade the panel
        StartCoroutine(FadePanel());
    }

    private IEnumerator FadePanel()
    {
        isFading = true;
        float elapsedTime = 0f;
        float startAlpha = brushPanelCanvasGroup.alpha;
        float targetAlpha = 1f - startAlpha;
        if (targetAlpha == 1f)
        {
            brushPanel.SetActive(true);
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            brushPanelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        if (targetAlpha == 0f)
        {
            brushPanel.SetActive(false);
            arDrawManager.AllowDraw(true);
        }

        brushPanelCanvasGroup.alpha = targetAlpha;
        isFading = false;
    }

    public void OnBrushWidthButtonClick()
    {
        if (isFading) return;
        brushSizeSlider.value = lineSettings.startWidth / baseWidthValue;
        brushSizeInputField.text = brushSizeSlider.value.ToString();
        arDrawManager.AllowDraw(false);
        StartCoroutine(FadePanel());
    }
}
