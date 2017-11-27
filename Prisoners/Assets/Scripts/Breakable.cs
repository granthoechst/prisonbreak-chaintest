using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    private float collisionForce;

    void Start()
    {
        collisionForce = 2000f;
    }

	void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Velocity: " + other.relativeVelocity.magnitude);
        Debug.Log("Mass: " + other.gameObject.GetComponent<Rigidbody2D>().mass);

        if (other.relativeVelocity.magnitude * other.gameObject.GetComponent<Rigidbody2D>().mass > collisionForce)
        {
            Destroy(gameObject);
        }
    }
}
