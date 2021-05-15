using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls the physics of legs axis
/// </summary>
public class LegsEngine : MonoBehaviour
{
    [SerializeField] Rigidbody engine;
    [SerializeField] Vector3 engineStartRotation;
    [SerializeField] float force;

    void Start()
    {
        engineStartRotation = engine.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        RotationSpeed();
    }

    /// <summary>
    /// Tries to keep z rotation speed around "force"
    /// </summary>
    private void RotationSpeed()
    {
        engine.angularVelocity = new Vector3(0, 0, -force);
    }

    /// <summary>
    /// Reset the rotation of legs axis (both legs parallels to the floor)
    /// </summary>
    public void ResetEngine()
    {
        engine.transform.localRotation = Quaternion.Euler(engineStartRotation);
    }
}
