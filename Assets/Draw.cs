using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public GameObject linePrefab;
    [HideInInspector]
    public GameObject currentLine;
    [HideInInspector]
    public LineRenderer drawLineRenderer;
    [HideInInspector]
    public List<Vector2> touchPositions;
    [HideInInspector]
    public Camera mainCam;

    private bool isDrawing;

    [SerializeField]
    private SplineMesh.Spline playerSpline;
    void Start()
    {
        mainCam = Camera.main;
        isDrawing = false;
    }

    void Update()
    {
        /*var tempTouchPos = Input.mousePosition;
        tempTouchPos.z = 10;
        if (mainCam.ScreenToWorldPoint(tempTouchPos).y < -0.4f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DrawLine();
            }
            if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(mainCam.ScreenToWorldPoint(tempTouchPos), touchPositions[touchPositions.Count - 1]) > .15f)
                {
                    UpdateLine(mainCam.ScreenToWorldPoint(tempTouchPos));
                }
            }
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            DrawLine();
        }
        if (Input.GetMouseButton(0))
        {
            if (Vector3.Distance(GetMousePosition(), touchPositions[touchPositions.Count - 1]) > .15f)
            {
                UpdateLine(GetMousePosition());
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
    Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 10;
    }
    private void SendAnchorPos()
    {
        isDrawing = false;
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        Vector3[] anchorPositions = new Vector3[drawLineRenderer.positionCount];
        drawLineRenderer.GetPositions(anchorPositions);
        //FindObjectOfType<PlayerCar>().GetAnchors(anchorPositions);

        SplineMesh.SplineSmoother smoother = playerSpline.transform.GetComponent<SplineMesh.SplineSmoother>();

        //Vector3 medianPoint = new Vector3();

        /*for(int i = 0; i < anchorPositions.Length; i++)
        {
            medianPoint += 
        }*/

        /*for(int i = 0; i < anchorPositions.Length; i++)
        {
            anchorPositions[i] += new Vector3(0, 0, -10);
        }*/

        SplineMesh.SplineNode node1 = new SplineMesh.SplineNode(anchorPositions[0], anchorPositions[0]);
        SplineMesh.SplineNode node2 = new SplineMesh.SplineNode(anchorPositions[1], anchorPositions[1]);
        playerSpline.FullReset(node1, node2);

        Debug.Log(anchorPositions[0] + " " + anchorPositions[1] + " " + anchorPositions[2]);
        for (int i = 2; i < anchorPositions.Length; i++)
        {

            SplineMesh.SplineNode node = new SplineMesh.SplineNode(anchorPositions[i] , anchorPositions[i]);

            playerSpline.AddNode(node);
        }
        playerSpline.RefreshCurves();
        smoother.SmoothAll();
        FindObjectOfType<PlayerCarGenerator>().GetTheMeshes();
    }

    private void DrawLine()
    {
        isDrawing = true;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        drawLineRenderer = currentLine.GetComponent<LineRenderer>();
        touchPositions.Clear();
        var tempTouchPos = Input.mousePosition;
        tempTouchPos.z = 10;
        /*touchPositions.Add(mainCam.ScreenToWorldPoint(tempTouchPos));
        touchPositions.Add(mainCam.ScreenToWorldPoint(tempTouchPos));*/
        touchPositions.Add(GetMousePosition());
        touchPositions.Add(GetMousePosition());
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
