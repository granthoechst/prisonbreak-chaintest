using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float topSpeed = 10f;
    bool facingRight = true;

    private void FixedUpdate()
    {
        float move;
        if (gameObject.tag == "Player1")
            move = Input.GetAxis("Horizontal2");
        else
            move = Input.GetAxis("Horizontal");

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * topSpeed, GetComponent<Rigidbody2D>().velocity.y);

        //if (move > 0 && !facingRight)
        //    Flip();
        //else if (move < 0 && facingRight)
        //    Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;

        theScale.x *= -1;

        transform.localScale = theScale;
    }

}
