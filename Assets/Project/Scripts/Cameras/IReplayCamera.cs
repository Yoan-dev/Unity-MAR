using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReplayCamera {

    void SetManager(ReplayCamerasManager manager);
    void SetActive(bool active);
}
