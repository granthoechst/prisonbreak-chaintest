using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMovement : MonoBehaviour {

    private bool chainPull;

	void OnCollisionEnter2D(Collision2D other)
    {
        
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(chainPull)
        {
            
        }
    }
}
