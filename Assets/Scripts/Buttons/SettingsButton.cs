using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private LineSettings lineSettings;
    [SerializeField] private TMP_InputField startWidth;
    [SerializeField] private TMP_InputField endWidth;
    [SerializeField] private TMP_InputField distanceFromCamera;
    [SerializeField] private TMP_InputField cornerVertices;
    [SerializeField] private TMP_InputField endCapVertices;
    [SerializeField] private TMP_InputField minDistanceBeforeNewPoint;
    [SerializeField] private Toggle allowMultiTouch;
    [SerializeField] private Toggle allowSimplification;
    [SerializeField] private TMP_InputField tolerance;
    [SerializeField] private TMP_InputField applySimplifyAfterPoints;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button resetButton;
    private ARDrawManager arDrawManager;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private float fadeDuration = 0.25f;
    private CanvasGroup settingsPanelCanvasGroup;
    private bool isFading = false;

    private void Awake() {
        arDrawManager = FindObjectOfType<ARDrawManager>();
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        resetButton.onClick.AddListener(OnResetButtonClick);
        settingsPanelCanvasGroup = settingsPanel.GetComponent<CanvasGroup>();
        allowSimplification.onValueChanged.AddListener(OnAllowSimplificationToggle);
    }

    private void OnConfirmButtonClick() {
        lineSettings.startWidth = float.Parse(startWidth.text);
        lineSettings.endWidth = float.Parse(endWidth.text);
        lineSettings.distanceFromCamera = float.Parse(distanceFromCamera.text);
        lineSettings.cornerVertices = int.Parse(cornerVertices.text);
        lineSettings.endCapVertices = int.Parse(endCapVertices.text);
        lineSettings.minDistanceBeforeNewPoint = float.Parse(minDistanceBeforeNewPoint.text);
        lineSettings.allowMultiTouch = allowMultiTouch.isOn;
        lineSettings.allowSimplification = allowSimplification.isOn;
        lineSettings.tolerance = float.Parse(tolerance.text);
        lineSettings.applySimplifyAfterPoints = float.Parse(applySimplifyAfterPoints.text);
        StartCoroutine(FadePanel());
    }

    private void OnCancelButtonClick() {
        StartCoroutine(FadePanel());
    }

    private void OnResetButtonClick() {
        startWidth.text = "0.005";
        endWidth.text = "0.005";
        distanceFromCamera.text = "0.3";
        cornerVertices.text = "10";
        endCapVertices.text = "10";
        minDistanceBeforeNewPoint.text = "0.001";
        allowMultiTouch.isOn = true;
        allowSimplification.isOn = false;
        tolerance.text = "0.01";
        applySimplifyAfterPoints.text = "1";
    }

    private void OnEnable() {
        startWidth.text = lineSettings.startWidth.ToString();
        endWidth.text = lineSettings.endWidth.ToString();
        distanceFromCamera.text = lineSettings.distanceFromCamera.ToString();
        cornerVertices.text = lineSettings.cornerVertices.ToString();
        endCapVertices.text = lineSettings.endCapVertices.ToString();
        minDistanceBeforeNewPoint.text = lineSettings.minDistanceBeforeNewPoint.ToString();
        allowMultiTouch.isOn = lineSettings.allowMultiTouch;
        allowSimplification.isOn = lineSettings.allowSimplification;
        tolerance.text = lineSettings.tolerance.ToString();
        applySimplifyAfterPoints.text = lineSettings.applySimplifyAfterPoints.ToString();
        OnAllowSimplificationToggle(allowSimplification.isOn);
    }

    private IEnumerator FadePanel() {
        isFading = true;
        float elapsedTime = 0f;
        float startAlpha = settingsPanelCanvasGroup.alpha;
        float targetAlpha = 1f - startAlpha;
        if (targetAlpha == 1f)
        {
            settingsPanel.SetActive(true);
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            settingsPanelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        if (targetAlpha == 0f)
        {
            settingsPanel.SetActive(false);
            arDrawManager.AllowDraw(true);
        }

        settingsPanelCanvasGroup.alpha = targetAlpha;
        isFading = false;
    }

    public void OnSettingsButtonClick() {
        if (!isFading) {
            settingsPanel.SetActive(true);
            arDrawManager.AllowDraw(false);
            StartCoroutine(FadePanel());
        }
    }

    private void OnAllowSimplificationToggle(bool isOn) {
        if (isOn) {
            tolerance.interactable = true;
            applySimplifyAfterPoints.interactable = true;
        } else {
            tolerance.interactable = false;
            applySimplifyAfterPoints.interactable = false;
        }
    }
}
