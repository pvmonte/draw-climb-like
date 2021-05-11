using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class DrawOnScreen : MonoBehaviour
{
    Camera cam;
    [SerializeField] Image drawImage;
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
        Time.timeScale = 0.1f;
        currentBrush = Instantiate(brushPrefab, transform);
        currentLineRenderer = currentBrush.GetComponent<LineRenderer>();
        Vector3 mousePosition = MousePositionInWorld();

        currentLineRenderer.SetPosition(0, mousePosition);
        currentLineRenderer.SetPosition(1, mousePosition);
    }

    public void UpdateBrush()
    {
        Vector3 mousePosition = MousePositionInWorld();        

        if (mousePosition != lastPosition)
        {
            AddPoint(mousePosition);
            lastPosition = mousePosition;
        }
    }

    private Vector3 MousePositionInWorld()
    {
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(mouseRay, out hit, 5))
        {
            return hit.point;
        }

        return lastPosition;
        /*
        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.z = transform.position.z;
        Vector3 mousePosition = cam.ScreenToWorldPoint(adjustedMousePos);
        mousePosition.z = transform.position.z;
        print($"mousePosition {mousePosition}");
        return mousePosition;
        */
    }

    void AddPoint(Vector3 pointPosition)
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
        currentLineRenderer.BakeMesh(wheelRightMesh.mesh, cam, true);
        currentLineRenderer.BakeMesh(wheelLeftMesh.mesh, cam, true);

        Vector3[] points = new Vector3[currentLineRenderer.positionCount];
        currentLineRenderer.GetPositions(points);
        

        OnDestroyMesh?.Invoke(points);

        ClearBrush();
        Time.timeScale = 1f;
    }

    private void AdjustLineRendererPositions()
    {
        Vector3 zero = currentLineRenderer.GetPosition(0);

        for (int i = 0; i < currentLineRenderer.positionCount; i++)
        {
            Vector3 currentPos = currentLineRenderer.GetPosition(i);
            var adjusted = currentPos - zero;
            adjusted.x *= 5;
            adjusted.y *= 5;
            currentLineRenderer.SetPosition(i, adjusted);
        }

        currentLineRenderer.startWidth = 0.2f;
        currentLineRenderer.endWidth = 0.2f;
    }

    private void ClearBrush()
    {
        Destroy(currentBrush);
        currentBrush = null;
        currentLineRenderer = null;
    }
}
