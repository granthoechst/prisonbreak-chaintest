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
				hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, distance);
				if (hit.collider != null && hit.collider.gameObject.tag == "Player2") {
					lifting = true;

				}
			}
			else {
				lifting = false;
			}
		}
		if (lifting) {
			hit.collider.gameObject.transform.position = holdPoint.position;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;

		Gizmos.DrawLine (transform.position, transform.position + Vector3.right * transform.localScale.x*distance);
	}
}
