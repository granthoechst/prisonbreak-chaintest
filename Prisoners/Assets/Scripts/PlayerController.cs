using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float topSpeed = 10f;
    public float jumpSpeed = 100f;
    bool facingRight = true;
    bool canAnchor = false;
    bool isAnchored = false;

    private float pivotAnchorOffset = 0.625f;

    private Transform anchorPoint;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float move;
        if (gameObject.tag == "Player1")
        {
            if (isAnchored)
            {
                if (Input.GetButtonDown("AnchorP1"))
                {
                    AnchorRelease();
                }
            }
            move = Input.GetAxis("Horizontal2");
            if (Input.GetButtonDown("Jump"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
            if (canAnchor)
            {
                if (Input.GetButtonDown("AnchorP1"))
                {
                    Anchor();
                }
            }
        }
            
        else
        {
            if(isAnchored)
            {
                if(Input.GetButtonDown("AnchorP2"))
                {
                    AnchorRelease();
                }       
            }
            move = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump2"))
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
            if(canAnchor)
            {
                if(Input.GetButtonDown("AnchorP2"))
                {
                    Anchor();
                }
            }
        }
            
        rb2d.velocity = new Vector2(move * topSpeed, rb2d.velocity.y);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        canAnchor = true;
        anchorPoint = other.transform;
        Debug.Log("Enter");
    }
    void OnTriggerExit2D(Collider2D other)
    {
        canAnchor = false;
        anchorPoint = null;
        Debug.Log("Exit");
    }

    void Anchor()
    {
        isAnchored = true;
        GetComponent<Rigidbody2D>().position = new Vector2(anchorPoint.transform.position.x - pivotAnchorOffset, anchorPoint.transform.position.y - pivotAnchorOffset);

        FixedJoint2D anchorJoint = this.gameObject.AddComponent<FixedJoint2D>();
        anchorJoint.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
    }

    void AnchorRelease()
    {
        isAnchored = false;
        Destroy(GetComponent<FixedJoint2D>());
    }
}
