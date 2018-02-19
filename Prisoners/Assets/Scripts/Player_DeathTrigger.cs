using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathTrigger : MonoBehaviour {

    private string tag_deathTrigger = "Death";

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == tag_deathTrigger)
        {
            Debug.Log("You died.");
            ReloadLevel();
        }
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
