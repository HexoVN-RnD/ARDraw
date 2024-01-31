using UnityEngine;

public class AppSettings : MonoBehaviour
{
    [SerializeField] private LineSettings lineSettings = null;
    [SerializeField] private int targetFrameRate = 120;
    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        lineSettings.LoadSettings();
    }
}
