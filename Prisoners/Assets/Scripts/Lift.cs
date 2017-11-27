using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

	public bool lifting;
	public float distance = 2f;
	RaycastHit2D hit;
	public Transform holdPoint;

	private void FixedUpdate() {
		if (Input.GetButton ("GrabBig")) {
			if (!lifting) {

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
		}
		if (lifting && Input.GetButton ("Up2")) 
		{
			lifting = false;
			hit.collider.gameObject.GetComponent<DistanceJoint2D> ().enableCollision = false;
			hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0,30);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;

		Gizmos.DrawLine (transform.position + new Vector3(0,1,0), transform.position + new Vector3(0,1,0) + Vector3.right * distance);
	}
}
