using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Builds the colliders of Legs
/// </summary>
public class LegsColliderBuilder : MonoBehaviour
{
    [SerializeField] GameObject legR;
    [SerializeField] GameObject legL;
    [SerializeField] PhysicMaterial physicMaterial;

    List<SphereCollider> colliders = new List<SphereCollider>();
    float legColliderRadius = 0.15f;

    [SerializeField] UnityEvent OnAddMeshToLegs;    

    /// <summary>
    /// Add mesh to legs by points given
    /// </summary>
    /// <param name="points">Points of LineRenderer used to draw legs</param>
    public void AddMeshToLegs(Vector3[] points)
    {
        ClearColliders();
        OnAddMeshToLegs?.Invoke();

        AddCollidersToMesh(points, legR);
        AddCollidersToMesh(points, legL);
    }

    /// <summary>
    /// Destroy legs colliders
    /// </summary>
    private void ClearColliders()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            Destroy(colliders[i]);
        }

        colliders.Clear();
    }

    /// <summary>
    /// Add colliders to legs mesh by given points
    /// </summary>
    /// <param name="points">Points used to add colliders</param>
    /// <param name="leg">The leg that is getting colliders</param>
    void AddCollidersToMesh(Vector3[] points, GameObject leg)
    {
        foreach (var point in points)
        {
            var col = leg.AddComponent<SphereCollider>();
            colliders.Add(col);
            col.center = point;
            col.radius = legColliderRadius;
            col.material = physicMaterial;
        }
    }
}
