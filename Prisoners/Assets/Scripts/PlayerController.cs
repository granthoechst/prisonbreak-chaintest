﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject grabbed;

    // width of the character sprites
    const float smallWidth = .625f;
    const float bigWidth = 1.06f;
    // basic movement constants
    const float bigSpeed = 10f;
    const float bigJumpSpeed = 30f;
    const float smallSpeed = 14f;
    const float smallJumpSpeed = 30f;
    // swinging - allow swinging without gravity defying
    static int swingFlagBig;
    static int swingFlagSmall;
    // climbing chain - int records which link climber is at
    static float climbLinkBig = 0;
    static float climbLinkSmall = 0;
    const float climbSpeed = 0.007f;
    private int numLinks = 0; // number of links in the chain, populated based on create chain script

    // universal speed cap - (soft)
    const float maxSpeed = 30f;

    private float pivotAnchorOffset = 0.5f;

    public bool facingRight = true;
    bool canAnchor = false;
    bool isAnchored = false;
    bool canGrab = false;
    bool isGrabbing = false;
    public bool isGrounded = false;
    public bool isWorldGrounded = false;

    private Transform anchorPoint;
    private Rigidbody2D rb2d;
    private Animator animator;
    private GameObject chain;
    private HingeJoint2D climbJoint;
    private Transform grabTrigger;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        chain = GameObject.Find("Chain");
        numLinks = chain.transform.childCount;
        // initialize climbing joint
        climbJoint = gameObject.AddComponent<HingeJoint2D>();
        climbJoint.connectedBody = chain.transform.GetChild(getLinkIndex(0)).gameObject.GetComponent<Rigidbody2D>();
        climbJoint.autoConfigureConnectedAnchor = false;
        climbJoint.connectedAnchor = new Vector2(2, 0);
        climbJoint.anchor = new Vector2(0.5f, 0.9f);
    }

    private void FixedUpdate()
    {
        isGrounded = IsCharacterGrounded();

        float horizontal = 0;
        // BIG GUY PLAYER 1
        if (gameObject.tag == "Player1")
        {
            // Movement (move, jump, crouch, swing)
            horizontal = Input.GetAxis("Horizontal1");
            if (isGrounded) // on the ground - side to side, jump, crouch, reset flags
            {
                animator.SetBool("Climbing", false);
                if (horizontal != 0)
                {
                    rb2d.velocity = new Vector2(horizontal * bigSpeed, rb2d.velocity.y);
                    animator.SetBool("Moving", true);
                } else
                {
                    animator.SetBool("Moving", false);
                }
                if (Input.GetButtonDown("Up1"))
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, bigJumpSpeed);
                }
                if (Input.GetButton("Crouch1"))
                {
                    rb2d.velocity = new Vector2(0, 0);
                }
                // reset swinging flags, disable climbing
                climbJoint.enabled = false;
                climbLinkBig = 0;
                swingFlagBig = 0;
            }
            else // In the air - swinging, climbing 
            {
                // sketchy normalization for swinging only
                float normalized = Mathf.Abs(horizontal) / horizontal;
                if (Input.GetButton("Horizontal1") && horizontal != 0 && (int)(normalized * 2) != swingFlagBig)
                {
                    rb2d.velocity = new Vector2(normalized * smallSpeed, rb2d.velocity.y);
                    swingFlagBig = (int)(normalized * 2);
                }
                // chain climbing - update climblink based on control
                int oldClimbLink = (int)climbLinkBig;
                if (Input.GetButton("Up1"))
                {
                    animator.SetBool("Climbing", true);
                    if (climbLinkBig < numLinks)
                    {
                        climbLinkBig += climbSpeed;
                    }
                }
                else if (Input.GetButton("GrabBig"))
                {
                    animator.SetBool("Climbing", true);
                    if (climbLinkBig > 0)
                    {
                        climbLinkBig -= climbSpeed;
                    }
                }
                else
                {
                    animator.SetBool("Climbing", false);
                }
                // rebuild the hingejoint according to climblink
                if (climbLinkBig <= 0)
                {
                    climbJoint.enabled = false;
                }
                else if (oldClimbLink != (int)climbLinkBig)
                {
                    climbJoint.enabled = true;
                    Rigidbody2D nextLink = chain.transform.GetChild(getLinkIndex(climbLinkBig)).gameObject.GetComponent<Rigidbody2D>();
                    climbJoint.connectedBody = nextLink;
                }
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
                    // this seems hella clumsy but idk how else to do this
                    foreach (HingeJoint2D joint in GetComponents<HingeJoint2D>())
                    {
                        if (joint.connectedBody.transform.parent == null || 
                            joint.connectedBody.transform.parent.tag != "Chain")
                        {
                            Destroy(joint);
                        }
                    }
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
            // Movement (move, jump, crouch, swing)
            horizontal = Input.GetAxis("Horizontal2");
            if (isGrounded) // on the ground - side to side, jump, crouch, reset flags
            {
                animator.SetBool("Climbing", false);
                if (horizontal != 0)
                {
                    rb2d.velocity = new Vector2(horizontal * smallSpeed, rb2d.velocity.y);
                    animator.SetBool("Moving", true);
                }
                else
                {
                    animator.SetBool("Moving", false);
                }
                if (Input.GetButtonDown("Up2"))
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, smallJumpSpeed);
                }
                if (Input.GetButton("Crouch2"))
                {
                    rb2d.velocity = new Vector2(0, 0);
                }
                // reset swinging flags, disable climbing
                climbJoint.enabled = false;
                climbLinkSmall = 0;
                swingFlagSmall = 0;
            }
            else // In the air - swinging, climbing 
            {
                // sketchy normalization for swinging only
                float normalized = Mathf.Abs(horizontal) / horizontal;
                if (Input.GetButton("Horizontal2") && horizontal != 0 && (int)(normalized * 2) != swingFlagSmall)
                {
                    rb2d.velocity = new Vector2(normalized * smallSpeed, rb2d.velocity.y);
                    swingFlagSmall = (int)(normalized * 2);
                }
                // chain climbing - update climblink based on control
                int oldClimbLink = (int)climbLinkSmall;
                if (Input.GetButton("Up2"))
                {
                    animator.SetBool("Climbing", true);
                    if (climbLinkSmall < numLinks)
                    {
                        climbLinkSmall += climbSpeed;
                    }
                }
                else if (Input.GetButton("GrabSmall"))
                {
                    animator.SetBool("Climbing", true);
                    if (climbLinkSmall > 0)
                    {
                        climbLinkSmall -= climbSpeed;
                    }
                }
                else
                {
                    animator.SetBool("Climbing", false);
                }
                // rebuild the hingejoint according to climblink
                if (climbLinkSmall <= 0)
                {
                    climbJoint.enabled = false;
                } else if (oldClimbLink != (int)climbLinkSmall)
                {
                    climbJoint.enabled = true;
                    Rigidbody2D nextLink = chain.transform.GetChild(getLinkIndex(climbLinkSmall)).gameObject.GetComponent<Rigidbody2D>();
                    climbJoint.connectedBody = nextLink;
                }                
            }

            // Grab actions
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
                    // this seems hella clumsy but idk how else to do this
                    foreach (HingeJoint2D joint in GetComponents<HingeJoint2D>())
                    {
                        if (joint.connectedBody.transform.parent == null ||
                            joint.connectedBody.transform.parent.tag != "Chain")
                        {
                            Destroy(joint);
                        }
                    }
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
        // implement soft universal speed cap
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        if (horizontal > 0 && !facingRight)
            Flip();
        else if (horizontal < 0 && facingRight)
            Flip();
    }
    // reset swinging ability when you collide with an object (less annoying)
    void OnCollisionExit2D(Collision2D other)
    {
        if (gameObject.tag == "Player1")
        {
            swingFlagBig = 0;
        } 
        else if (gameObject.tag == "Player2")
        {
            swingFlagSmall = 0;
        }
    }

    private int getLinkIndex(float dist)
    {
        if (gameObject.tag == "Player2")
        {
            return (int)(numLinks - 1 - dist);
        } else
        {
            return (int)dist;
        }
    }

    private void Grabbing()
    {
        // disallow grabs if the character is only grounded on that object it is grabbing
        if (!IsCharacterGrounded(grabTrigger.parent.gameObject))
        {
            return;
        }
        isGrabbing = true;
        HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.connectedBody = grabTrigger.parent.gameObject.GetComponent<Rigidbody2D>();
		grabbed = grabTrigger.parent.gameObject;

        if (grabTrigger.gameObject.tag == "Left")
        {
            rb2d.position = new Vector2(grabTrigger.parent.transform.position.x - grabTrigger.parent.transform.GetComponent<CapsuleCollider2D>().size.x, this.transform.position.y);
            hingeJoint.anchor = new Vector2(transform.GetComponent<CapsuleCollider2D>().size.x, transform.GetComponent<CapsuleCollider2D>().size.y / 2);
            hingeJoint.connectedAnchor = new Vector2(0, grabTrigger.parent.GetComponent<CapsuleCollider2D>().size.y / 2);
        }
        else if (grabTrigger.gameObject.tag == "Right")
        {
            rb2d.position = new Vector2(grabTrigger.parent.transform.position.x + grabTrigger.parent.transform.GetComponent<CapsuleCollider2D>().size.x, this.transform.position.y);
            hingeJoint.anchor = new Vector2(0, transform.GetComponent<CapsuleCollider2D>().size.y / 2);
            hingeJoint.connectedAnchor = new Vector2(grabTrigger.parent.GetComponent<CapsuleCollider2D>().size.x, grabTrigger.parent.GetComponent<CapsuleCollider2D>().size.y / 2);
        }
    }

    // param exclude is an object that does not count as ground
    private bool IsCharacterGrounded(GameObject exclude = null)
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = gameObject.transform.position;
        rightRayStart = leftRayStart;

        // assign right ray according to player sprite width
        if (gameObject.tag == "Player2")
        {
            rightRayStart.x += smallWidth;
        }
        else
        {
            rightRayStart.x += bigWidth;
        }


        Debug.DrawRay(leftRayStart, Vector3.down, Color.blue);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.blue);

        float rayLength = GetComponent<CapsuleCollider2D>().size.y / 2 + 0.1f;
        //float rayLength = GetComponent<CapsuleCollider2D>().size.y / 2 + 0.1f;
        // Check if below object is part of physical environment layer
        int mask = 1 << LayerMask.NameToLayer("World");
        mask |= 1 << LayerMask.NameToLayer("Draggable");

        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayStart, Vector2.down, rayLength, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayStart, Vector2.down, rayLength, mask);

        // the purpose of this was to tell the game small guy is grounded while lifted by the big one
        bool lifted = false;
        if (gameObject.tag == "Player2")
        {
            RaycastHit2D hitBigL = Physics2D.Raycast(leftRayStart, Vector2.down, rayLength, 1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D hitBigR = Physics2D.Raycast(rightRayStart, Vector2.down, rayLength, 1 << LayerMask.NameToLayer("Default"));
            // checks that it is actually lifting, not just overlapping, by checking that the players have the same velocity
            if (hitBigL.collider != null && hitBigL.collider.gameObject.GetComponent<Rigidbody2D>())
            {
                lifted = hitBigL.collider.gameObject.tag == "Player1";
                // orient lifted little guy same way as big guy
                if (lifted && (Mathf.Sign(hitBigL.collider.gameObject.transform.localScale.x) != Mathf.Sign(transform.localScale.x)))
                {
                    Flip();
                }
            }
            else if (hitBigR.collider != null && hitBigR.collider.gameObject.GetComponent<Rigidbody2D>())
            {
                lifted = hitBigR.collider.gameObject.tag == "Player1";
                // orient lifted little guy same way as big guy
                if (lifted && (Mathf.Sign(hitBigR.collider.gameObject.transform.localScale.x) != Mathf.Sign(transform.localScale.x)))
                {
                    Flip();
                }
            }
        }
        // not grounded if both rays only hit the excluded object or no object
        if (exclude != null)
        {
            Collider2D left = hitLeft.collider;
            Collider2D right = hitRight.collider;
            //Debug.Log(exclude.transform);
            //Debug.Log(right.transform.parent);
            //Debug.Log(left.transform.parent);
            if ((left == null || left.transform.parent == exclude.transform) &&
                (right == null || right.transform.parent == exclude.transform))
            {
                Debug.Log(exclude.transform);
                Debug.Log(left.transform.parent);
                Debug.Log(right.transform.parent);
                return false;
            }
        }
        // only count lifted as grounded if no object is excluded
        return hitLeft.collider || hitRight.collider || lifted;
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;

        theScale.x *= -1;
        transform.localScale = theScale;

        Vector3 pos = transform.position;
        float halfWidth = .625f;
        if(gameObject.tag == "Player1")
        {
            halfWidth = 1f;
        }

        if (facingRight)
        {
            pos.x -= halfWidth;
        } else
        {
            pos.x += halfWidth;
        }
        transform.position = pos;
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
        GetComponent<Rigidbody2D>().position = new Vector2(anchorPoint.transform.position.x, anchorPoint.transform.position.y - pivotAnchorOffset);
        animator.SetBool("Moving", false);
        FixedJoint2D anchorJoint = this.gameObject.AddComponent<FixedJoint2D>();
        anchorJoint.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
    }

    void AnchorBig()
    {
        isAnchored = true;
        GetComponent<Rigidbody2D>().position = new Vector2(anchorPoint.transform.position.x, anchorPoint.transform.position.y - 2*pivotAnchorOffset);
        animator.SetBool("Moving", false);
        FixedJoint2D anchorJoint = this.gameObject.AddComponent<FixedJoint2D>();
        anchorJoint.connectedBody = anchorPoint.GetComponent<Rigidbody2D>();
    }

    void AnchorRelease()
    {
        isAnchored = false;
        Destroy(GetComponent<FixedJoint2D>());
    }
}
