using UnityEngine;

// classic checkpoint everywhere on the circuit
public class Checkpoint : AbstractCheckpoint
{
    public override void CheckpointSuccess(GameObject player)
    {
        // we save the coordinates of the player
        // to teleport it back if he goes the wrong way
        position = player.transform.position;
        rotation = player.transform.eulerAngles;
    }

    public override bool IsStart()
    {
        return false;
    }
}