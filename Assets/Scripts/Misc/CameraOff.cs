using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnCameraOff", 3);
    }

    private void TurnCameraOff()
    {
        gameObject.SetActive(false);
    }

}
