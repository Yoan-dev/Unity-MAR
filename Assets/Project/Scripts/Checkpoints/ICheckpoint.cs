using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpoint {

    void SetManager(CheckpointManager manager);
    void CheckpointSuccess(GameObject player);
    bool IsStart();
    Vector3 GetPosition();
    Vector3 GetRotation();
    GameObject GetGameObject();
}
