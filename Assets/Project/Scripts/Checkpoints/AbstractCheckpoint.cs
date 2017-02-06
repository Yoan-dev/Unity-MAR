using UnityEngine;

// checkpoint template
public abstract class AbstractCheckpoint : MonoBehaviour, ICheckpoint
{
    protected CheckpointManager manager;
    protected Vector3 position;
    protected Vector3 rotation;
    
    public abstract bool IsStart();

    // if the checkpoint is valid when triggered by the player
    public abstract void CheckpointSuccess(GameObject player);

    public void SetManager(CheckpointManager manager)
    {
        this.manager = manager;
    }

    // detection of the car
    void OnTriggerEnter(Collider collider)
    {
        if (manager == null) return;
        if (manager.TriggerCheckpoint(GetComponent<ICheckpoint>()))
        {
            CheckpointSuccess(collider.gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetRotation()
    {
        return rotation;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
