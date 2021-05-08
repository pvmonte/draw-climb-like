using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    Mesh mesh;

    public Transform[] verticesRef;
    public float verticesOffset;
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();
    }

    private void CreateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        for (int i = 0; i < verticesRef.Length; i++)
        {
            MakeMeshData(verticesRef[i].position, vertices.Count);
        }

        DrawMesh();
        gameObject.AddComponent<MeshCollider>();
    }    

    void MakeMeshData(Vector3 reference, int startingIndex)
    {
        vertices.Add(reference);
        vertices.Add(reference + new Vector3(0, verticesOffset, 0));

        CreateTrianglesArray();
    }

    void CreateTrianglesArray()
    {
        int trianglesTotal = (vertices.Count - 2) * 3;

        for (int i = 1; i < verticesRef.Length; i++)
        {
            triangles.Add(i * 2);
            triangles.Add(i * 2 - 2);
            triangles.Add(i * 2 + 1);

            triangles.Add(i * 2 + 1);
            triangles.Add(i * 2 - 2);
            triangles.Add(i * 2  - 1);
        }
    }

    void DrawMesh()
    {
        mesh.Clear();        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
