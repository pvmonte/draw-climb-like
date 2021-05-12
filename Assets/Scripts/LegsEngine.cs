using UnityEngine;
using UnityEngine.Events;

public class LegsEngine : MonoBehaviour
{
    [SerializeField] Rigidbody engine;
    [SerializeField] Vector3 engineStartRotation;
    [SerializeField] float force;

    [SerializeField] GetLegSizeEvent OnGetLegSize;

    [System.Serializable]
    public class GetLegSizeEvent : UnityEvent<Vector3> { }

    void Start()
    {
        engineStartRotation = engine.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        engine.angularVelocity = new Vector3(0, 0, -force);
    }

    public void ResetEngine()
    {
        engine.transform.localRotation = Quaternion.Euler(engineStartRotation);
    }

    public void CalculateLegsSize()
    {
        var mesh = GetComponentInChildren<MeshFilter>();
        Vector3 legsBounds = mesh.mesh.bounds.size;
        OnGetLegSize?.Invoke(legsBounds);
    }
}
