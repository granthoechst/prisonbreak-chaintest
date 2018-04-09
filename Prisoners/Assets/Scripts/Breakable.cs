using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public float collisionForce = 20000f;
    public float breakMass = 200f;

    void Start()
    {
        Debug.Log("Velocity: ");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Velocity: " + other.relativeVelocity.magnitude);
        //Debug.Log("Mass: " + other.gameObject.GetComponent<Rigidbody2D>().mass);

        if (other.relativeVelocity.magnitude * other.gameObject.GetComponent<Rigidbody2D>().mass > collisionForce)
        {
            Destroy(gameObject);
        }
        if (other.gameObject.GetComponent<Rigidbody2D>().mass > breakMass)
        {
            Destroy(gameObject);
        }
    }
}
