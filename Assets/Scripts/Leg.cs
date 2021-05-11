using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    [SerializeField] Rigidbody bodyRb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            print("on");
            bodyRb.velocity = new Vector3(bodyRb.velocity.x, 0, bodyRb.velocity.z);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            print("off");
            bodyRb.velocity = new Vector3(bodyRb.velocity.x, -10f, bodyRb.velocity.z);
            print(bodyRb.velocity);
        }
    }
}
