using UnityEngine;

/// <summary>
/// Constrols physics of players body
/// </summary>
public class PlayerPhysics : MonoBehaviour
{        
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Adds horizontal force to body
    /// </summary>
    public void AddHorizontalForce()
    {
        rb.AddForce(new Vector3(10, 0, 0));
    }

    /// <summary>
    /// Reduces the mass of body.
    /// Used to make easy to go upstairs
    /// </summary>
    public void ZeroGravity()
    {
        rb.mass = 0.1f;
    }


    /// <summary>
    /// Raises body mass.
    /// Used to prevent body from been thrown away by legs spin
    /// </summary>
    public void ResetGravity()
    {
        rb.mass = 10;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
