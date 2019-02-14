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
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        Vector3 pos = transform.position;
        ceilingCheck = Physics2D.Raycast(pos + new Vector3(.1f, .2f, 0), Vector2.up, distance, 1 << LayerMask.NameToLayer("World"));
        if (Input.GetButton ("GrabBig")) {   
            if (!lifting && !ceilingCheck)
            {
                Physics2D.queriesStartInColliders = false;
                /* hit = Physics2D.Linecast (pos + new Vector3(-.5f, 0.4f, 0), pos + new Vector3(1.5f, 0.4f, 0), 1 << LayerMask.NameToLayer("Default"));
                Debug.Log(hit.collider);
                if (hit.collider != null && hit.collider.gameObject.tag == "Player2") {
					lifting = true;
				}*/
                if (player2 && Vector3.Distance(pos + new Vector3(.5f, 0, 0), player2.transform.position) < 1.2f)
                {
                    lifting = true;
                }
			}
		}
		if (lifting) {
			player2.GetComponent<Rigidbody2D>().position = holdPoint.position;
			player2.GetComponent<DistanceJoint2D>().enableCollision = true;
            animator.SetBool("Lifting", true);
        }
		if (lifting && (Input.GetButton ("Up2") || ceilingCheck))
		{
			lifting = false;
			player2.GetComponent<DistanceJoint2D> ().enableCollision = false;
			player2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,30);
            animator.SetBool("Lifting", false);
        }
	}

	void OnDrawGizmos(){
        Vector3 pos = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos + new Vector3(-.5f, 0.4f, 0), pos + new Vector3(1.5f, 0.4f, 0));
    }
}
