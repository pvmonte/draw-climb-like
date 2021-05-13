using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Draw on screen using a LineRenderer
/// </summary>
public class DrawOnScreen : MonoBehaviour
{
    Camera cam;
    [SerializeField] Image drawImage;
    [SerializeField] MeshFilter wheelRight;
    [SerializeField] MeshFilter wheelLeft;
    [SerializeField] GameObject brushPrefab;

    GameObject currentBrush;
    LineRenderer currentLineRenderer;
    Vector3 lastPosition;

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

    /// <summary>
    /// Create an instance of brush
    /// </summary>
    public void CreateBrush()
    {
        //Pauses time while drawing
        TimeController.PauseTime();

        currentBrush = Instantiate(brushPrefab, transform);
        currentLineRenderer = currentBrush.GetComponent<LineRenderer>();
        Vector3 mousePosition = PointerPositionInWorld();

        //sets the two first points to the same coordinates to prevent some visual bugs
        currentLineRenderer.SetPosition(0, mousePosition);
        currentLineRenderer.SetPosition(1, mousePosition);
    }

    /// <summary>
    /// Add point to brush by pointer position
    /// </summary>
    public void UpdateBrush()
    {
        Vector3 mousePosition = PointerPositionInWorld();        

        //Only add point if pointer has moved
        if (mousePosition != lastPosition)
        {
            AddPoint(mousePosition);
            lastPosition = mousePosition;
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

        if(Physics.Raycast(mouseRay, out hit, 5))
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
        AdjustLineRendererPositions();

        BakeLegsMesh();

        Vector3[] points = new Vector3[currentLineRenderer.positionCount];
        currentLineRenderer.GetPositions(points);

        OnDestroyMesh?.Invoke(points);

        ClearBrush();

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
}

public static class TimeController
{
    public static void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public static void RestoreTimeSpeed()
    {
        Time.timeScale = 1f;
    }
}
