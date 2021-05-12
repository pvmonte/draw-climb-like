using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{    
    [SerializeField] float maxGravityTimer;
    float gravityTimer;
    bool isGravityTimerRunning;
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if(gravityTimer <= 0)
        {
            //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            return;
        }

        //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (isGravityTimerRunning)
            gravityTimer -= Time.deltaTime;
    }

    public void ResetGravityTimer()
    {
        gravityTimer = maxGravityTimer;
        isGravityTimerRunning = true;
    }

    public void StopGravityTimer()
    {
        isGravityTimerRunning = false;
    }

    public void AddHorizontalForce()
    {
        rb.AddForce(new Vector3(10, 0, 0));
    }

    public void ZeroGravity()
    {
        rb.mass = 0.1f;
    }

    public void ResetGravity()
    {
        rb.mass = 10;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
