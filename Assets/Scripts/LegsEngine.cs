using UnityEngine;

public class LegsEngine : MonoBehaviour
{
    [SerializeField] Rigidbody engine;
    [SerializeField] Vector3 engineStartRotation;
    [SerializeField] float force;

    void Start()
    {
        engineStartRotation = engine.transform.localEulerAngles;
        //wheelL.AddTorque(0, 0, 2);
        //wheelR.AddTorque(0, 0, 2);
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
}
