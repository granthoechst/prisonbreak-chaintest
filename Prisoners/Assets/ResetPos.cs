using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour {

    // Update is called once per frame
    Transform startPos;

	void Update () {
		if(Input.GetButtonDown("ResetPos"))
        {
            transform.position = new Vector3(-32f, 7f, 0f);
        }
	}
}
