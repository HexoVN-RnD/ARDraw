using UnityEngine;

public class AppSettings : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 120;
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
