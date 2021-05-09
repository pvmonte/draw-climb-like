using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawOnScreen : MonoBehaviour
{
    Camera cam;
    [SerializeField] GameObject brush;

    [SerializeField] LineRenderer currentLineRenderer;

    [SerializeField] Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateBrush();
            print("create");
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 adjustedMousePos = Input.mousePosition;
            adjustedMousePos.z = 10;
            Vector3 mousePosition = cam.ScreenToWorldPoint(adjustedMousePos);     

            if(mousePosition != lastPosition)
            {
                AddPoint(mousePosition);
                lastPosition = mousePosition;
            }

            print("add");
        }

        if(Input.GetMouseButtonUp(0))
        {
            currentLineRenderer = null;

            print("destroy");
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector3 adjustedMousePos = Input.mousePosition;
        adjustedMousePos.z = 10;
        Vector3 mousePosition = cam.ScreenToWorldPoint(adjustedMousePos);

        currentLineRenderer.SetPosition(0, mousePosition);
        currentLineRenderer.SetPosition(1, mousePosition);
    }

    void AddPoint(Vector2 pointPosition)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPosition);
    }
}
