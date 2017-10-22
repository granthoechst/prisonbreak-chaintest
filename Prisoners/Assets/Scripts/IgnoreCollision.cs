using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1" ||
            collision.gameObject.tag == "Player2" ||
            collision.gameObject.tag == "Chain")
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
    }
}
