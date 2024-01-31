using System.Collections.Generic;
using DilmerGames.Core.Singletons;
using HSVPicker;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    [SerializeField]
    private LineSettings lineSettings = null;

    [SerializeField]
    private UnityEvent OnDraw = null;

    [SerializeField]
    private Camera arCamera = null;

    private List<ARAnchor> anchors = new List<ARAnchor>();

    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();

    private bool CanDraw { get; set; }

    private Stack<ARLine> undoStack = new Stack<ARLine>();
    private Stack<ARLine> redoStack = new Stack<ARLine>();

    public ColorPicker colorPicker;

    void Update()
    {
#if !UNITY_EDITOR
        DrawOnTouch();
#else
        DrawOnMouse();
#endif
    }

    public void AllowDraw(bool isAllow)
    {
        CanDraw = isAllow;
    }

    void DrawOnTouch()
    {
        if (!CanDraw) return;

        if (Input.touchCount == 0) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is over a UI element
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                // If a UI element is selected, ignore the touch
                return;
            }
        }

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            if (i >= Input.touchCount) return;

            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));

            //ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began)
            {
                OnDraw?.Invoke();

                //ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));
                var anchorObject = new GameObject("MyARAnchor");
                anchorObject.transform.position = touchPosition;
                anchorObject.transform.rotation = Quaternion.identity;
                ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();

                if (anchor == null)
                    Debug.LogError("Error creating reference point");
                else
                {
                    anchors.Add(anchor);
                    ARDebugManager.Instance.LogInfo($"Anchor created & total of {anchors.Count} anchor(s)");
                }

                ARLine line = new ARLine(lineSettings);
                Lines.Add(touch.fingerId, line);
                undoStack.Push(line);
                redoStack.Clear();

                line.AddNewLineRenderer(transform, anchor, touchPosition);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Lines[touch.fingerId].AddPoint(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Lines[touch.fingerId].FinishLine();
                Lines.Remove(touch.fingerId);
            }
        }
    }

    void DrawOnMouse()
    {
        if (!CanDraw) return;

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // If the touch is over a UI element, ignore it
                return;
            }

            OnDraw?.Invoke();

            if (Lines.Keys.Count == 0)
            {
                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                undoStack.Push(line);
                redoStack.Clear();

                line.AddNewLineRenderer(transform, null, mousePosition);
            }
            else
            {
                Lines[0].AddPoint(mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            try
            {
                Lines[0].FinishLine();
                Lines.Remove(0);
            }
            catch (KeyNotFoundException e)
            {
                Debug.Log("Key not found");
            }
        }
    }

    GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }
        //Lines.Clear();
    }

    public void UndoLastLine()
    {
        if (undoStack.Count > 0)
        {
            ARLine line = undoStack.Pop();
            if (line.LineObject == null)
            {
                return;
            }
            line.LineObject.SetActive(false);
            redoStack.Push(line);
        }
    }

    public void RedoLastLine()
    {
        if (redoStack.Count > 0)
        {
            ARLine line = redoStack.Pop();
            if (line.LineObject == null)
            {
                return;
            }
            line.LineObject.SetActive(true);
            undoStack.Push(line);
        }
    }

    public void ChangeLineColor()
    {
        // Get the selected color from the color picker
        Color selectedColor = colorPicker.CurrentColor;

        // Apply the color to the line settings
        lineSettings.startColor = selectedColor;
        lineSettings.endColor = selectedColor;
    }

    public Color GetLineStartColor()
    {
        return lineSettings.startColor;
    }

    public void ChangeLineWidth(float width)
    {
        lineSettings.startWidth = width;
        lineSettings.endWidth = width;
    }

    public void ChangeLineMaterial(Material material)
    {
        lineSettings.lineMaterial = material;
    }
}