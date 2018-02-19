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
            furthestCheckpoint = Player_CheckpointFlag.player_FurthestCheckpointReached;
            if (furthestCheckpoint == null)
            {
                ReloadCurrentLevel();
            }
            else
            {
                Debug.Log(furthestCheckpoint.name);
                CheckpointLoad_Player();
            }
        }
    }

    void ReloadCurrentLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void CheckpointLoad_Player()
    {
        Debug.Log("Checkpoint transform: " + furthestCheckpoint.transform.position);
        player1Reference.transform.position = furthestCheckpoint.transform.position + new Vector3(-1.0f,0,0);
        player2Reference.transform.position = furthestCheckpoint.transform.position + new Vector3(1.0f,0,0);
    }
}
