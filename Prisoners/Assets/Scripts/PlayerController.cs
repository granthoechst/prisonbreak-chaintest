using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float topSpeed = 10f;
    public float jumpSpeed = 100f;
    private float pivotAnchorOffset = 0.625f;

    bool facingRight = true;
    bool canAnchor = false;
    bool isAnchored = false;
    bool canGrab = false;
    bool isGrabbing = false;
    public bool isGrounded = false;

    private Transform anchorPoint;
    private Rigidbody2D rb2d;
    private Transform grabTrigger;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = IsCharacterGrounded();

        float move;
        // BIG GUY PLAYER 1
        if (gameObject.tag == "Player1")
        {
            if (isAnchored)
            {
                if (Input.GetButtonDown("GrabBig"))
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
                if (Input.GetButtonDown("GrabBig"))
                {
                    AnchorBig();
                }
            }
            if (canGrab)
            {
                if (Input.GetButton("GrabBig"))
                {

                }
            }
        }
        
        // SMALL GUY PLAYER 2    
        else
        {
            if (isAnchored)
            {
                if(Input.GetButtonDown("GrabSmall"))
                {
                    AnchorRelease();
                }       
            }
            else if (isGrabbing)
            {
                if(!Input.GetButton("GrabSmall"))
                {
                    isGrabbing = false;
                    Destroy(GetComponent<HingeJoint2D>());
                }
            }
            
            move = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump2") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            }
            if(canAnchor)
            {
                if(Input.GetButtonDown("GrabSmall"))
                {
                    AnchorSmall();
                }
            }
            if(canGrab)
            {
                if(Input.GetButton("GrabSmall"))
                {
                    isGrabbing = true;
                    HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();
                    hingeJoint.connectedBody = grabTrigger.parent.gameObject.GetComponent<Rigidbody2D>();

                    if (grabTrigger.gameObject.tag == "Left")
                    {
                        rb2d.position = new Vector2(grabTrigger.parent.transform.position.x - grabTrigger.parent.transform.GetComponent<BoxCollider2D>().size.x, this.transform.position.y);
                        hingeJoint.anchor = new Vector2(transform.GetComponent<BoxCollider2D>().size.x, transform.GetComponent<BoxCollider2D>().size.y / 2);
                        hingeJoint.connectedAnchor = new Vector2(0, grabTrigger.parent.GetComponent<BoxCollider2D>().size.y / 2);
                    }
                    else if (grabTrigger.gameObject.tag == "Right")
                    {
                        rb2d.position = new Vector2(grabTrigger.parent.transform.position.x + grabTrigger.parent.transform.GetComponent<BoxCollider2D>().size.x, this.transform.position.y);
                        hingeJoint.anchor = new Vector2(0, transform.GetComponent<BoxCollider2D>().size.y / 2);
                        hingeJoint.connectedAnchor = new Vector2(grabTrigger.parent.GetComponent<BoxCollider2D>().size.x, grabTrigger.parent.GetComponent<BoxCollider2D>().size.y / 2);
                    }
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
        // Draggable
        if(other.gameObject.layer == 10) // Draggable layer
        {
            canGrab = true;
            other.transform.parent.GetComponentInChildren<MeshRenderer>().enabled = true;
            grabTrigger = other.transform; // we grab the child trigger that we hit
        }
        // Anchor
        if(other.gameObject.layer == 11) // Mount layer
        {
            canAnchor = true;
            anchorPoint = other.transform;
            Debug.Log("Enter");
        }   
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Draggable
        if (other.gameObject.layer == 10) // Draggable layer
        {
            canGrab = false;
            other.transform.parent.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        // Anchor
        if(other.gameObject.layer == 11) // Mount layer
        {
            canAnchor = false;
            anchorPoint = null;     
        }         
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
