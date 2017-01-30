using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpoint {

    void SetManager(CheckpointManager manager);
    bool IsStart();
}
