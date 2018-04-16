using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnLever : MonoBehaviour {
    public bool off = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("asdf");
        if (off)
        {
            GetComponent<SpriteRenderer>().flipY = true;
            off = false;
        }
    }
}
