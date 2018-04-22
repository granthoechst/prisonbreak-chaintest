using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            Destroy(other.gameObject);
        }
    }
}
