using UnityEngine;

// special camera allowing to switch to another
// used to switch from menu to game
public class Camera : MonoBehaviour {

    public GameObject otherCamera;

    public void Switch()
    {
        otherCamera.SetActive(true);
        gameObject.SetActive(false);
    }
}
