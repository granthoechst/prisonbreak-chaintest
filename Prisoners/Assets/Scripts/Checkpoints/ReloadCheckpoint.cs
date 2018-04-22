using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadCheckpoint : MonoBehaviour {

    public GameObject furthestCheckpoint;
    public GameObject player1Reference;
    public GameObject player2Reference;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CheckpointLoad_Player();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentLevel();
        }
    }

    void ReloadCurrentLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void CheckpointLoad_Player()
    {
        furthestCheckpoint = Player_CheckpointFlag.player_FurthestCheckpointReached;
        if (furthestCheckpoint == null)
        {
            ReloadCurrentLevel();
        }
        else
        {

            Debug.Log("Checkpoint transform: " + furthestCheckpoint.transform.position);
            player1Reference.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player2Reference.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player1Reference.transform.position = furthestCheckpoint.transform.position + new Vector3(-1.0f, 0, 0);
            player2Reference.transform.position = furthestCheckpoint.transform.position + new Vector3(1.0f, 0, 0);
        }
    }
}
