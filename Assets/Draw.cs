using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject currentLine;
    public LineRenderer drawLineRenderer;
    public List<Vector2> touchPositions;
    public Camera mainCam;

    private bool isDrawing;

    void Start()
    {
        mainCam = Camera.main;
        isDrawing = false;
    }

    void Update()
    {
        var tempTouchPos = Input.mousePosition;
        tempTouchPos.z = 10;
        if (mainCam.ScreenToWorldPoint(tempTouchPos).y < -0.4f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DrawLine();
            }
            if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(mainCam.ScreenToWorldPoint(tempTouchPos), touchPositions[touchPositions.Count - 1]) > .25f)
                {
                    UpdateLine(mainCam.ScreenToWorldPoint(tempTouchPos));
                }
            }
        }
        if (Input.touches.Length > 0 && isDrawing)
        {
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                // Send data to 3D Drawer
                SendAnchorPos();
                Debug.Log("Data sent.");
                Destroy(currentLine);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {

            // Send data to 3D Drawer
            SendAnchorPos();
            Debug.Log("Data sent.");
            Destroy(currentLine);
        }
    }

    private void SendAnchorPos()
    {
        isDrawing = false;
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        Vector3[] anchorPositions = new Vector3[drawLineRenderer.positionCount];
        drawLineRenderer.GetPositions(anchorPositions);
        FindObjectOfType<PlayerCar>().GetAnchors(anchorPositions);
        SplineMesh.Spline spline = FindObjectOfType<SplineMesh.Spline>();
        for (int i = 0; i < anchorPositions.Length; i++)
        {
            SplineMesh.SplineNode node = new SplineMesh.SplineNode(anchorPositions[i] , Vector3.zero);

            spline.AddNode(node);
        }
        spline.RefreshCurves();
    }

    private void DrawLine()
    {
        isDrawing = true;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        touchPositions.Clear();
        var tempTouchPos = Input.mousePosition;
        tempTouchPos.z = 10;
        touchPositions.Add(mainCam.ScreenToWorldPoint(tempTouchPos));
        touchPositions.Add(mainCam.ScreenToWorldPoint(tempTouchPos));
        drawLineRenderer.SetPosition(0, touchPositions[0]);
        drawLineRenderer.SetPosition(1, touchPositions[1]);
    }

    private void UpdateLine(Vector2 newTouchPos)
    {
        touchPositions.Add(newTouchPos);
        drawLineRenderer.positionCount++;
        drawLineRenderer.SetPosition(drawLineRenderer.positionCount - 1, newTouchPos);
    }
}
