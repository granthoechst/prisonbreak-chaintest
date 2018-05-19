using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

	public bool lifting;
	public float distance = 4f;
    RaycastHit2D ceilingCheck;
    RaycastHit2D hit;
	public Transform holdPoint;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        ceilingCheck = Physics2D.Raycast(transform.position + new Vector3(.1f, .2f, 0), Vector2.up, distance, 1 << LayerMask.NameToLayer("World"));
        if (Input.GetButton ("GrabBig")) {   
            if (!lifting && !ceilingCheck)
            {
                Physics2D.queriesStartInColliders = false;
				hit = Physics2D.Raycast (transform.position + new Vector3(0,1,0), Vector2.right, distance, 1 << LayerMask.NameToLayer("Default"));
				if (hit.collider != null && hit.collider.gameObject.tag == "Player2") {
					lifting = true;
				}
			}
		}
		if (lifting) {
			hit.collider.attachedRigidbody.position = holdPoint.position;
			hit.collider.gameObject.GetComponent<DistanceJoint2D> ().enableCollision = true;
            animator.SetBool("Lifting", true);
        }
		if (lifting && (Input.GetButton ("Up2") || ceilingCheck))
		{
			lifting = false;
			hit.collider.gameObject.GetComponent<DistanceJoint2D> ().enableCollision = false;
			hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,30);
            animator.SetBool("Lifting", false);
        }
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;

		Gizmos.DrawLine (transform.position + new Vector3(0,1,0), transform.position + new Vector3(0,1,0) + Vector3.right * distance);
	}
}
