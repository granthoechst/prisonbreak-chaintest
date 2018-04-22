using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Order : MonoBehaviour
{

    public int checkpointIndex = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
