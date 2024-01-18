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
    [SerializeField] private Toggle allowLineSimplification;
    [SerializeField] private TMP_InputField lineSimplificationTolerance;
    [SerializeField] private Toggle allowPointSimplification;
    [SerializeField] private TMP_InputField pointSimplificationTolerance;
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
        allowPointSimplification.onValueChanged.AddListener(OnAllowPointSimplificationToggle);
        allowLineSimplification.onValueChanged.AddListener(OnAllowLineSimplificationToggle);
    }

    private void OnConfirmButtonClick() {
        lineSettings.startWidth = float.Parse(startWidth.text);
        lineSettings.endWidth = float.Parse(endWidth.text);
        lineSettings.distanceFromCamera = float.Parse(distanceFromCamera.text);
        lineSettings.cornerVertices = int.Parse(cornerVertices.text);
        lineSettings.endCapVertices = int.Parse(endCapVertices.text);
        lineSettings.minDistanceBeforeNewPoint = float.Parse(minDistanceBeforeNewPoint.text);
        lineSettings.allowMultiTouch = allowMultiTouch.isOn;
        lineSettings.allowLineSimplification = allowLineSimplification.isOn;
        lineSettings.lineSimplificationTolerance = float.Parse(lineSimplificationTolerance.text);
        lineSettings.allowPointSimplification = allowPointSimplification.isOn;
        lineSettings.pointSimplificationTolerance = float.Parse(pointSimplificationTolerance.text);
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
        allowLineSimplification.isOn = false;
        lineSimplificationTolerance.text = "0.01";
        allowPointSimplification.isOn = false;
        pointSimplificationTolerance.text = "0.01";
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
        allowLineSimplification.isOn = lineSettings.allowLineSimplification;
        lineSimplificationTolerance.text = lineSettings.lineSimplificationTolerance.ToString();
        allowPointSimplification.isOn = lineSettings.allowPointSimplification;
        pointSimplificationTolerance.text = lineSettings.pointSimplificationTolerance.ToString();
        applySimplifyAfterPoints.text = lineSettings.applySimplifyAfterPoints.ToString();
        OnAllowPointSimplificationToggle(allowPointSimplification.isOn);
        OnAllowLineSimplificationToggle(allowLineSimplification.isOn);
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

    private void OnAllowPointSimplificationToggle(bool isOn) {
        if (isOn) {
            pointSimplificationTolerance.interactable = true;
            applySimplifyAfterPoints.interactable = true;
        } else {
            pointSimplificationTolerance.interactable = false;
            applySimplifyAfterPoints.interactable = false;
        }
    }

    private void OnAllowLineSimplificationToggle(bool isOn) {
        if (isOn) {
            lineSimplificationTolerance.interactable = true;
        } else {
            lineSimplificationTolerance.interactable = false;
        }
    }
}
