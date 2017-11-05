using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float topSpeed = 10f;
    public float jumpSpeed = 100f;
    bool facingRight = true;
    bool canAnchor = false;
    bool isAnchored = false;

    public bool isGrounded = false;

    private float pivotAnchorOffset = 0.625f;

    private Transform anchorPoint;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = IsCharacterGrounded();

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
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
            if (canAnchor)
            {
                if (Input.GetButtonDown("AnchorP1"))
                {
                    AnchorBig();
                }
            }
        }
            
        else
        {
            Debug.Log(isGrounded);

            if (isAnchored)
            {
                if(Input.GetButtonDown("AnchorP2"))
                {
                    AnchorRelease();
                }       
            }
            move = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump2") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
            if(canAnchor)
            {
                if(Input.GetButtonDown("AnchorP2"))
                {
                    AnchorSmall();
                }
            }
        }
            
        rb2d.velocity = new Vector2(move * topSpeed, rb2d.velocity.y);

        //if (move > 0 && !facingRight)
        //    Flip();
        //else if (move < 0 && facingRight)
        //    Flip();
    }

    private bool IsCharacterGrounded()
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = gameObject.transform.position;
        rightRayStart = leftRayStart;
        rightRayStart.x += 2 * pivotAnchorOffset;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.blue);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.blue);

        // Check if below object is part of physical environment layer
        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayStart, Vector2.down, GetComponent<BoxCollider2D>().size.y / 2 + 0.1f, 1 << LayerMask.NameToLayer("World"));
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayStart, Vector2.down, GetComponent<BoxCollider2D>().size.y / 2 + 0.1f, 1 << LayerMask.NameToLayer("World"));

        if (hitLeft)
        {
            return true;
        }
        if (hitRight)
        {
            return true;
        }
        return false;
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

    void AnchorSmall()
    {
        isAnchored = true;
        GetComponent<Rigidbody2D>().position = new Vector2(anchorPoint.transform.position.x - pivotAnchorOffset, anchorPoint.transform.position.y - pivotAnchorOffset);

        FixedJoint2D anchorJoint = this.gameObject.AddComponent<FixedJoint2D>();
        anchorJoint.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
    }

    void AnchorBig()
    {
        isAnchored = true;
        GetComponent<Rigidbody2D>().position = new Vector2(anchorPoint.transform.position.x - 2*pivotAnchorOffset, anchorPoint.transform.position.y - 2*pivotAnchorOffset);

        FixedJoint2D anchorJoint = this.gameObject.AddComponent<FixedJoint2D>();
        anchorJoint.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
    }

    void AnchorRelease()
    {
        isAnchored = false;
        Destroy(GetComponent<FixedJoint2D>());
    }
}
