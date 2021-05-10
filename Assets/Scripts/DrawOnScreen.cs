using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DrawOnScreen : MonoBehaviour
{
    Camera cam;
    [SerializeField] GameObject wheelRight;
    [SerializeField] GameObject wheelLeft;
    [SerializeField] GameObject brushPrefab;

    GameObject currentBrush;
    LineRenderer currentLineRenderer;
    Vector3 lastPosition;

    [SerializeField] OnDestroyMeshEvent OnDestroyMesh;

    [System.Serializable]
    public class OnDestroyMeshEvent : UnityEvent<Vector3[]> { }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    public void CreateBrush()
    {
        currentBrush = Instantiate(brushPrefab);
        currentLineRenderer = currentBrush.GetComponent<LineRenderer>();

        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.z = 10;
        Vector3 mousePosition = cam.ScreenToWorldPoint(adjustedMousePos);

        currentLineRenderer.SetPosition(0, mousePosition);
        currentLineRenderer.SetPosition(1, mousePosition);
    }

    public void UpdateBrush()
    {
        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.z = 10;
        Vector3 mousePosition = cam.ScreenToWorldPoint(adjustedMousePos);
        //mousePosition.z = 0;

        if (mousePosition != lastPosition)
        {
            AddPoint(mousePosition);
            lastPosition = mousePosition;
        }
    }

    void AddPoint(Vector2 pointPosition)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPosition);
    }

    public void DestroyBrush()
    {
        AdjustLineRendererPositions();
        
        var wheelRightMesh = wheelRight.GetComponent<MeshFilter>();
        var wheelLeftMesh = wheelLeft.GetComponent<MeshFilter>();
        currentLineRenderer.Simplify(0.05f);
        currentLineRenderer.BakeMesh(wheelRightMesh.mesh, Camera.main, true);
        currentLineRenderer.BakeMesh(wheelLeftMesh.mesh, Camera.main, true);

        Vector3[] points = new Vector3[currentLineRenderer.positionCount];
        currentLineRenderer.GetPositions(points);
        

        OnDestroyMesh?.Invoke(points);

        ClearBrush();
    }

    private void AdjustLineRendererPositions()
    {
        Vector3 zero = currentLineRenderer.GetPosition(0);

        for (int i = 0; i < currentLineRenderer.positionCount; i++)
        {
            Vector3 currentPos = currentLineRenderer.GetPosition(i);
            currentLineRenderer.SetPosition(i, currentPos - zero);
        }
    }

    private void ClearBrush()
    {
        Destroy(currentBrush);
        currentBrush = null;
        currentLineRenderer = null;
    }
}
