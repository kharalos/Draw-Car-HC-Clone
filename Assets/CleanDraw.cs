using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanDraw : MonoBehaviour
{
    [SerializeField]
    private Camera drawCam;

    public GameObject linePrefab;
    [HideInInspector]
    public GameObject currentLine;
    [HideInInspector]
    public LineRenderer drawLineRenderer;
    [HideInInspector]
    public List<Vector3> touchPositions;
    [HideInInspector]
    public List<Vector3> vertexPositions;
    [SerializeField]
    private RectTransform panel;

    [SerializeField]
    private Car car;

    private bool isDrawing;

    private int screenWidth, screenHeight;

    [SerializeField]
    private SplineMesh.Spline playerSpline;

    private float dpLeft, dpRight, dpUp, dpDown;
    void Start()
    {
        isDrawing = false;
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        Debug.Log("Screen Height is " + screenHeight + " Screen Width is " + screenWidth);
        dpLeft = panel.anchorMin.x;
        dpRight = panel.anchorMax.x;
        dpUp = panel.anchorMax.y;
        dpDown = panel.anchorMin.y;
    }
    
    Vector3 firstPos;
    void Update()
    {
        var tempTouchPos = Input.mousePosition;
        
        Debug.DrawRay(drawCam.ScreenToViewportPoint(tempTouchPos), -drawCam.transform.forward, Color.red);
        if (tempTouchPos.x < screenWidth * dpRight
            && tempTouchPos.x > screenWidth * dpLeft
            && tempTouchPos.y < screenHeight * dpUp
            && tempTouchPos.y > screenHeight * dpDown
           )

        {
            //Coverts Mouse Position to Accurate World Space
            Vector3 vertPos = tempTouchPos;
            vertPos.x -= screenWidth / 2;
            vertPos.y -= screenHeight * dpDown + ((dpUp - dpDown) / 2);
            vertPos *= 3;
            vertPos.z = 0;

            Vector3 drawPos = tempTouchPos;
            drawPos.z = 1;
            drawPos = drawCam.ScreenToWorldPoint(drawPos);

            if (Input.GetMouseButtonDown(0))
            {
                firstPos = transform.position;
                FirstPoint(drawCam.ScreenToViewportPoint(vertPos));
                FirstLine(drawPos);
            }
            if (Input.GetMouseButton(0))
            {
                if (vertexPositions.Count > 0)
                {
                    if (Vector3.Distance(drawCam.ScreenToViewportPoint(tempTouchPos), vertexPositions[vertexPositions.Count - 1]) > .03f)
                    {
                        AddPoint(drawCam.ScreenToViewportPoint(vertPos));
                        UpdateLine(drawPos - (transform.position - firstPos));
                    }
                }
            }
        }
        if (Input.touches.Length > 0 && isDrawing)
        {
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                TouchReleased();
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            TouchReleased();
        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, car.transform.position + new Vector3(0,5,-8), 0.5f);
    }
    private void TouchReleased()
    {
        // Send data to 3D Drawer
        SendAnchorPos();
        SendMeshVerts();
        Debug.Log("Data sent.");
        Destroy(currentLine);
        touchPositions.Clear();
        vertexPositions.Clear();
    }
    private void SendMeshVerts()
    {

        SplineMesh.SplineSmoother smoother = playerSpline.transform.GetComponent<SplineMesh.SplineSmoother>();


        SplineMesh.SplineNode node1 = new SplineMesh.SplineNode(vertexPositions[0], vertexPositions[0]);
        SplineMesh.SplineNode node2 = new SplineMesh.SplineNode(vertexPositions[1], vertexPositions[1]);
        playerSpline.FullReset(node1, node2);

        for (int i = 2; i < vertexPositions.Count; i++)
        {
            SplineMesh.SplineNode node = new SplineMesh.SplineNode(vertexPositions[i], vertexPositions[i]);

            playerSpline.AddNode(node);
        }
        playerSpline.RefreshCurves();
        smoother.SmoothAll();
        car.GetData(vertexPositions[0], vertexPositions[vertexPositions.Count - 1]);
    }
    private void SendAnchorPos()
    {
        isDrawing = false;
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        Vector3[] anchorPositions = new Vector3[drawLineRenderer.positionCount];
        drawLineRenderer.GetPositions(anchorPositions);
    }


    private void FirstLine(Vector3 newTouchPos)
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, transform);
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        drawLineRenderer.positionCount++;
        isDrawing = true;
        touchPositions.Clear();
        touchPositions.Add(newTouchPos);
        drawLineRenderer.SetPosition(0, touchPositions[0]);
    }
    private void UpdateLine(Vector3 newTouchPos)
    {
        touchPositions.Add(newTouchPos);
        drawLineRenderer.positionCount++;
        drawLineRenderer.SetPosition(drawLineRenderer.positionCount - 1, newTouchPos);
    }
    private void FirstPoint(Vector3 point)
    {
        vertexPositions.Clear();
        vertexPositions.Add(point);
    }
    private void AddPoint(Vector3 point)
    {
        vertexPositions.Add(point);
    }
}
