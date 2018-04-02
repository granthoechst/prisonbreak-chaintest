using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {
    public GameObject item;                // The enemy prefab to be spawned.
    public float spawnTime = 8f;            // How long between each spawn.
    public GameObject lever; // lever to control spawner

    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        // If the spawner is off
        if (lever && lever.GetComponent<spawnLever>().off)
        {
            return;
        }
        // Create an instance of the item prefab right below the spawner
        Instantiate(item, this.transform.position, this.transform.rotation);
    }
}
