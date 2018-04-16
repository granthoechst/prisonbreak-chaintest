using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathTrigger : MonoBehaviour {

    private string tag_deathTrigger = "Death";
    public GameObject checkpointReloader;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tag_deathTrigger)
        {
            Debug.Log("You died.");
            // Can change this to CheckpointLoad_Player in ReloadCheckpoint.cs
            checkpointReloader.GetComponent<ReloadCheckpoint>().CheckpointLoad_Player();
            //ReloadLevel();
        }
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }


}
