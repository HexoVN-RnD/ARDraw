using UnityEngine;

[CreateAssetMenu(fileName = "LineSettings", menuName = "Create Line Settings", order = 0)]
public class LineSettings : ScriptableObject
{
    public string lineTagName = "Line";

    public Color startColor = Color.white;

    public Color endColor = Color.white;

    public float startWidth = 0.01f;

    public float endWidth = 0.01f;

    public float distanceFromCamera = 0.3f;

    public Material lineMaterial;

    public int cornerVertices = 5;

    public int endCapVertices = 5;

    public enum TextureMode
    {
        Stretch,
        Tile,
        DistributePerSegment,
        RepeatPerSegment,
        Static
    }

    public LineTextureMode textureMode = (LineTextureMode)TextureMode.Stretch;

    public Vector2 textureScale = Vector2.one;

    [Range(0, 1.0f)]
    public float minDistanceBeforeNewPoint = 0.001f;
    public bool allowMultiTouch = true;

    [Header("Tolerance Options")]
    public bool allowLineSimplification = false;
    public float lineSimplificationTolerance = 0.001f;
    public bool allowPointSimplification = false;

    public float pointSimplificationTolerance = 0.001f;

    public float applySimplifyAfterPoints = 20.0f;

    public void SaveSettings()
    {
        PlayerPrefs.SetString("lineTagName", lineTagName);
        PlayerPrefs.SetString("startColor", ColorUtility.ToHtmlStringRGBA(startColor));
        PlayerPrefs.SetString("endColor", ColorUtility.ToHtmlStringRGBA(endColor));
        PlayerPrefs.SetFloat("startWidth", startWidth);
        PlayerPrefs.SetFloat("endWidth", endWidth);
        PlayerPrefs.SetFloat("distanceFromCamera", distanceFromCamera);
        PlayerPrefs.SetString("lineMaterial", lineMaterial.name);
        PlayerPrefs.SetInt("cornerVertices", cornerVertices);
        PlayerPrefs.SetInt("endCapVertices", endCapVertices);
        PlayerPrefs.SetInt("textureMode", (int)textureMode);
        PlayerPrefs.SetString("textureScale", JsonUtility.ToJson(textureScale));
        PlayerPrefs.SetFloat("minDistanceBeforeNewPoint", minDistanceBeforeNewPoint);
        PlayerPrefs.SetInt("allowMultiTouch", allowMultiTouch ? 1 : 0);
        PlayerPrefs.SetInt("allowLineSimplification", allowLineSimplification ? 1 : 0);
        PlayerPrefs.SetFloat("lineSimplificationTolerance", lineSimplificationTolerance);
        PlayerPrefs.SetInt("allowPointSimplification", allowPointSimplification ? 1 : 0);
        PlayerPrefs.SetFloat("pointSimplificationTolerance", pointSimplificationTolerance);
        PlayerPrefs.SetFloat("applySimplifyAfterPoints", applySimplifyAfterPoints);
    }

    public void LoadSettings()
    {
        lineTagName = PlayerPrefs.GetString("lineTagName", lineTagName);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("startColor", ""), out startColor);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("endColor", ""), out endColor);
        startWidth = PlayerPrefs.GetFloat("startWidth", startWidth);
        endWidth = PlayerPrefs.GetFloat("endWidth", endWidth);
        distanceFromCamera = PlayerPrefs.GetFloat("distanceFromCamera", distanceFromCamera);
        string materialName = PlayerPrefs.GetString("lineMaterial", "");
        lineMaterial = materialName != "" ? Resources.Load<Material>(materialName) : lineMaterial;
        cornerVertices = PlayerPrefs.GetInt("cornerVertices", cornerVertices);
        endCapVertices = PlayerPrefs.GetInt("endCapVertices", endCapVertices);
        textureMode = (LineTextureMode)PlayerPrefs.GetInt("textureMode", (int)textureMode);
        textureScale = JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString("textureScale", JsonUtility.ToJson(textureScale)));
        minDistanceBeforeNewPoint = PlayerPrefs.GetFloat("minDistanceBeforeNewPoint", minDistanceBeforeNewPoint);
        allowMultiTouch = PlayerPrefs.GetInt("allowMultiTouch", allowMultiTouch ? 1 : 0) == 1;
        allowLineSimplification = PlayerPrefs.GetInt("allowLineSimplification", allowLineSimplification ? 1 : 0) == 1;
        lineSimplificationTolerance = PlayerPrefs.GetFloat("lineSimplificationTolerance", lineSimplificationTolerance);
        allowPointSimplification = PlayerPrefs.GetInt("allowPointSimplification", allowPointSimplification ? 1 : 0) == 1;
        pointSimplificationTolerance = PlayerPrefs.GetFloat("pointSimplificationTolerance", pointSimplificationTolerance);
        applySimplifyAfterPoints = PlayerPrefs.GetFloat("applySimplifyAfterPoints", applySimplifyAfterPoints);
    }

    public void SaveMaterial()
    {
        PlayerPrefs.SetString("lineMaterial", lineMaterial.name);
    }

    public void SaveColor()
    {
        PlayerPrefs.SetString("startColor", ColorUtility.ToHtmlStringRGBA(startColor));
        PlayerPrefs.SetString("endColor", ColorUtility.ToHtmlStringRGBA(endColor));
    }

    public void SaveWidth()
    {
        PlayerPrefs.SetFloat("startWidth", startWidth);
        PlayerPrefs.SetFloat("endWidth", endWidth);
    }

    public void SaveTextureScale()
    {
        PlayerPrefs.SetString("textureScale", JsonUtility.ToJson(textureScale));
    }
}