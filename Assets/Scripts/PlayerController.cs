using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody wheelR;
    [SerializeField] Rigidbody wheelL;

    // Start is called before the first frame update
    void Start()
    {
        wheelL.AddTorque(0, 0, 2);
        wheelR.AddTorque(0, 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //wheelL.AddTorque(0, 0, -1);
        //wheelR.AddTorque(0, 0, -1);
        //wheelL.angularVelocity = new Vector3(0, 0, -2);
        //wheelR.angularVelocity = new Vector3(0, 0, -2);
        wheelL.transform.Rotate(0, 0, -90 * Time.deltaTime);
        wheelR.transform.Rotate(0, 0, -90 * Time.deltaTime);
    }

    public void AddMeshToWheels()
    {
        var collider = wheelR.gameObject.AddComponent<MeshCollider>();
        collider.convex = true;
        collider = wheelL.gameObject.AddComponent<MeshCollider>();
        collider.convex = true;
    }
}
