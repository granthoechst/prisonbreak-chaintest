using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // basic movement constants
    const float bigSpeed = 10f;
    const float bigJumpSpeed = 20f;
    const float smallSpeed = 15f;
    const float smallJumpSpeed = 30f;
    // swinging - allow swinging without gravity defying
    static int swingFlagBig;
    static int swingFlagSmall;
    // universal speed cap
    const float maxSpeed = 30f;

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

        float horizontal = 0;
        // BIG GUY PLAYER 1
        if (gameObject.tag == "Player1")
        {
            // basic movement (move, jump, crouch, swing)
            horizontal = Input.GetAxis("Horizontal1");
            // sketchy normalization for swinging only
            float normalized = Mathf.Abs(horizontal) / horizontal;
            if (isGrounded)
            {
                if (horizontal != 0)
                {
                    rb2d.velocity = new Vector2(horizontal * bigSpeed, rb2d.velocity.y);
                }
                swingFlagBig = 0;
            }
            else if (Input.GetButton("Horizontal1") && horizontal != 0 && (int)(normalized * 2) != swingFlagBig)
            {
                rb2d.velocity = new Vector2(normalized * bigSpeed, rb2d.velocity.y);
                swingFlagBig = (int)(normalized * 2);
            }
            if (Input.GetButtonDown("Up1") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, bigJumpSpeed);
            }
            if (Input.GetButton("Crouch1") && isGrounded)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
            // grab actions
            if (isAnchored)
            {
                if (Input.GetButtonUp("GrabBig"))
                {
                    AnchorRelease();
                }
            }
            else if (isGrabbing)
            {
                if (!Input.GetButton("GrabBig"))
                {
                    isGrabbing = false;
                    Destroy(GetComponent<HingeJoint2D>());
                }
            }
            if (canAnchor)
            {
                if (Input.GetButton("GrabBig"))
                {
                    AnchorBig();
                }
            }
            if (canGrab)
            {
                if (Input.GetButton("GrabBig"))
                {
                    Grabbing();
                }
            }
        }
        
        // SMALL GUY PLAYER 2    
        else if (gameObject.tag == "Player2")
        {
            // basic movement (move, jump, crouch, swing)
            horizontal = Input.GetAxis("Horizontal2");
            // sketchy normalization for swinging only
            float normalized = Mathf.Abs(horizontal) / horizontal;
            if (isGrounded)
            {
                if (horizontal != 0)
                {
                    rb2d.velocity = new Vector2(horizontal * smallSpeed, rb2d.velocity.y);
                }
                swingFlagSmall = 0;
            }
            else if (Input.GetButton("Horizontal2") && horizontal != 0 && (int)(normalized * 2) != swingFlagSmall)
            {
                rb2d.velocity = new Vector2(normalized * smallSpeed, rb2d.velocity.y);
                swingFlagSmall = (int)(normalized * 2);
            }
            if (Input.GetButtonDown("Up2") && isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, smallJumpSpeed);
            }
            if (Input.GetButton("Crouch2") && isGrounded)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
            // grab actions
            if (isAnchored)
            {
                if (Input.GetButtonUp("GrabSmall"))
                {
                    AnchorRelease();
                }
            }
            else if (isGrabbing)
            {
                if (!Input.GetButton("GrabSmall"))
                {
                    isGrabbing = false;
                    Destroy(GetComponent<HingeJoint2D>());
                }
            }
            if (canAnchor)
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
                    Grabbing();
                }
            }
        }
        // implement universal speed cap
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxSpeed);
        }
        //if (move > 0 && !facingRight)
        //    Flip();
        //else if (move < 0 && facingRight)
        //    Flip();
    }

    private void Grabbing()
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

    private bool IsCharacterGrounded()
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = gameObject.transform.position;
        rightRayStart = leftRayStart;
        rightRayStart.x += 2 * pivotAnchorOffset;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.blue);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.blue);

        float rayLength = GetComponent<BoxCollider2D>().size.y / 2 + 0.1f;
        //float rayLength = GetComponent<BoxCollider2D>().size.y / 2 + 0.1f;
        // Check if below object is part of physical environment layer
        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayStart, Vector2.down, rayLength, 1 << LayerMask.NameToLayer("World"));
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayStart, Vector2.down, rayLength, 1 << LayerMask.NameToLayer("World"));

        return hitLeft || hitRight;
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
