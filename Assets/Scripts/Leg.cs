using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Holds legs events
/// </summary>
public class Leg : MonoBehaviour
{
    [SerializeField] UnityEvent OnStartCollision;
    [SerializeField] UnityEvent OnEndCollision;

    private void OnCollisionEnter(Collision collision)
    {
        //On collide with floor
        if(collision.gameObject.CompareTag("Floor"))
        {
            OnStartCollision?.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //while colliding with floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            OnStartCollision?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //On end collision with floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            OnEndCollision?.Invoke();
        }
    }
}
