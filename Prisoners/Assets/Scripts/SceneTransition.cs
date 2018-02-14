using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    private string tagTransition = "Transition";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == tagTransition)
        {
            int levelInt = other.gameObject.GetComponent<LoadLevel>().levelToLoad;
            ChangeScene(levelInt);
        }
    }

    void ChangeScene(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        Debug.Log("Loaded level index" + levelIndex);
    }
}
