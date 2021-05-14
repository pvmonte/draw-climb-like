using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeshBrush : MonoBehaviour
{
    Camera cam;
    [SerializeField] MeshFilter wheelRight;
    [SerializeField] MeshFilter wheelLeft;
    [SerializeField] GameObject brushPrefab;

    GameObject currentBrush;
    LineRenderer currentLineRenderer;
    Vector3 lastPosition;

    [SerializeField] int currentPoint = 0;
    [SerializeField] List<Vector3> points;
    [SerializeField] float brushSpeed;
    int frame = 0;
    bool isdrawing;

    List<Action> actions = new List<Action>();

    /// <summary>
    /// Event called when mesh is destroyed.Passes an Vector3 Array to methos in event
    /// </summary>
    [SerializeField] OnDestroyMeshEvent OnDestroyMesh;

    [System.Serializable]
    public class OnDestroyMeshEvent : UnityEvent<Vector3[]> { }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        frame++;
    }

    /// <summary>
    /// Create an instance of brush
    /// </summary>
    public void CreateBrush()
    {
        //Pauses time while drawing
        TimeController.PauseTime();

        
        Vector3 mousePosition = PointerPositionInWorld();

        //sets the two first points to the same coordinates to prevent some visual bugs
        points.Add(mousePosition);
        brushPrefab.transform.position = points[0];
        brushPrefab.SetActive(true);

        if (points.Count > 1)
        {
            StartCoroutine(WalkThroughPoints());
        }
    }

    /// <summary>
    /// Add point to brush by pointer position
    /// </summary>
    public void UpdateBrush()
    {
        if (frame % 4 == 0)
        {
            Vector3 mousePosition = PointerPositionInWorld();

            //Only add point if pointer has moved
            if (mousePosition != lastPosition)
            {
                points.Add(mousePosition);
                lastPosition = mousePosition;

                if (points.Count > 1 && !isdrawing)
                {
                    StartCoroutine(WalkThroughPoints());
                }
            }
        }
    }

    /// <summary>
    /// Casts a ray from screen to Canvas to set the points of LineRenderer
    /// </summary>
    /// <returns></returns>
    private Vector3 PointerPositionInWorld()
    {
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, 5))
        {
            return hit.point;
        }

        return lastPosition;
    }

    /// <summary>
    /// Add point to LineRenderer
    /// </summary>
    /// <param name="pointPosition"></param>
    void AddPoint(Vector3 pointPosition)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPosition);
    }

    /// <summary>
    /// Ends the bush lifecicle
    /// </summary>
    public void EndBrushLifeCicle()
    {
        StartCoroutine(ClearTheBrush());
       
        //Restore time
        TimeController.RestoreTimeSpeed();
    }

    /// <summary>
    /// Create the mesh of legs
    /// </summary>
    private void BakeLegsMesh()
    {
        //Reduces the amount of points in LineRenderer
        currentLineRenderer.Simplify(0.05f);

        //Create the mesh of legs
        currentLineRenderer.BakeMesh(wheelRight.mesh, cam, true);
        currentLineRenderer.BakeMesh(wheelLeft.mesh, cam, true);
    }

    /// <summary>
    /// Adjust LineRenderer points to world proportions
    /// </summary>
    private void AdjustLineRendererPositions()
    {
        //The position of first point of brush LineRenderer
        Vector3 pointZero = currentLineRenderer.GetPosition(0);

        //Loop through all points
        for (int i = 0; i < currentLineRenderer.positionCount; i++)
        {
            Vector3 currentPos = currentLineRenderer.GetPosition(i);

            //Subtract pointZero from all point to assert they will drawn from the center of leg object
            var adjusted = currentPos - pointZero;

            //Multiplay to get a better proportion in world size
            adjusted.x *= 10;
            adjusted.y *= 10;

            //Reinsert in LineRenderer points array
            currentLineRenderer.SetPosition(i, adjusted);
        }

        //Adjust width to better proportions in world size
        currentLineRenderer.startWidth = 0.5f;
        currentLineRenderer.endWidth = 0.5f;
    }

    /// <summary>
    /// Clear brush in this object
    /// </summary>
    private void ClearBrush()
    {
        Destroy(currentBrush);
        currentBrush = null;
        currentLineRenderer = null;
    }

    IEnumerator WalkThroughPoints()
    {
        isdrawing = true;

        while (currentPoint < points.Count - 1)
        {
            yield return LerpBetweenTwoPoints(currentPoint);

            while (currentPoint == points.Count - 1)
            {
                print("waiting new point");
                yield return null;
            }

            if(currentPoint < points.Count - 1)
            currentPoint++;
        }
    }

    IEnumerator LerpBetweenTwoPoints(int pos1)
    {
        Vector3 dist = points[pos1 + 1] - brushPrefab.transform.position;
        brushPrefab.transform.right = dist;

        float lerpRate = 0;

        while (lerpRate < 1)
        {
            print($"lerpRate {lerpRate}");
            brushPrefab.transform.position = Vector3.Lerp(points[pos1], points[pos1 + 1], lerpRate);
            lerpRate += Time.unscaledDeltaTime * 20;
            yield return null;
        }

        brushPrefab.transform.position = points[pos1 + 1];
    }

    IEnumerator ClearTheBrush()
    {
        while (currentPoint < points.Count)
        {
            yield return null;
        }

        print("clear");
        isdrawing = false;
        currentPoint = 0;
        points.Clear();
        brushPrefab.SetActive(false);
    }
}
