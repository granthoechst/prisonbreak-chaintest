using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CheckpointFlag : MonoBehaviour
{
    private string tag_checkpointFlag = "CheckpointFlag";

    // this script exists on both player1 and player2
    // static->if one player hits a checkpoint trigger, both instances share these variables
    public static GameObject player_FurthestCheckpointReached;
    public static int player_FurthestCheckpoint_index;

    void OnTriggerEnter2D(Collider2D other)
    {
        // a player just reached a checkpoint
        if (other.tag == tag_checkpointFlag)
        {   
            if (player_FurthestCheckpointReached == null)
            {
                player_FurthestCheckpointReached = other.gameObject;
                player_FurthestCheckpoint_index = other.gameObject.GetComponent<Checkpoint_Order>().checkpointIndex;
            }
            else
            {
                GameObject mostRecentCheckpoint = other.gameObject;
                int mostRecentCheckpointIndex = mostRecentCheckpoint.GetComponent<Checkpoint_Order>().checkpointIndex;
                if (mostRecentCheckpointIndex > player_FurthestCheckpoint_index)
                {
                    player_FurthestCheckpointReached = other.gameObject;
                    player_FurthestCheckpoint_index = other.gameObject.GetComponent<Checkpoint_Order>().checkpointIndex;
                }
            }
            Debug.Log("Reached Checkpoint: " + player_FurthestCheckpointReached.name);
        }
    }
}