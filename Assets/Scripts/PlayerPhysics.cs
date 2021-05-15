using UnityEngine;

/// <summary>
/// Constrols physics of players body
/// </summary>
public class PlayerPhysics : MonoBehaviour
{        
    Rigidbody rb;
    [SerializeField] float horizontalForce = 10;
    [SerializeField] float maxHorizontalSpeed;

    float lightMass = 0.1f;
    float heavyMass = 10;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Adds horizontal force to body
    /// </summary>
    public void AddHorizontalForce()
    {
        rb.AddForce(new Vector3(horizontalForce, 0, 0));

        if(rb.velocity.x > maxHorizontalSpeed)
        rb.velocity = new Vector3(maxHorizontalSpeed, rb.velocity.y, 0);
    }

    /// <summary>
    /// Reduces the mass of body.
    /// Used to make easy to go upstairs
    /// </summary>
    public void ZeroGravity()
    {
        rb.mass = lightMass;
    }


    /// <summary>
    /// Raises body mass.
    /// Used to prevent body from been thrown away by legs spin
    /// </summary>
    public void ResetGravity()
    {
        rb.mass = heavyMass;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
