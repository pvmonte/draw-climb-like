using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{    
    [SerializeField] GameObject wheelR;
    [SerializeField] GameObject wheelL;
    [SerializeField] PhysicMaterial physicMaterial;
    [SerializeField] float verticalSpeed;
    [SerializeField] float horizontalSpeed;

    List<SphereCollider> colliders = new List<SphereCollider>();

    [SerializeField] UnityEvent OnAddMeshToWheels;

    private void Update()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
    }

    public void AddMeshToWheels(Vector3[] points)
    {
        ResetColliders();
        OnAddMeshToWheels?.Invoke();

        AddCollidersToMesh(points, wheelR.gameObject);
        AddCollidersToMesh(points, wheelL.gameObject);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void ResetColliders()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            Destroy(colliders[i]);
        }

        colliders.Clear();
    }

    void AddCollidersToMesh(Vector3[] points, GameObject leg)
    {
        foreach (var point in points)
        {
            var col = leg.AddComponent<SphereCollider>();
            colliders.Add(col);
            col.center = point;
            col.radius = 0.15f;
            //col.material = physicMaterial;
        }
    }
}
