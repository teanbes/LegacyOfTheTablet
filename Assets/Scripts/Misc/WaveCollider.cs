using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollider : MonoBehaviour
{
    [SerializeField] private GameObject waveCollider;
    [SerializeField] private GameObject cineCamera;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStateMachine>())
        {
            Invoke("SetCameraTrue", 0.5f);

            Invoke("TurnOnObject", 1.5f);

            Invoke("SetCameraFalse", 4.0f);

        }
    }

    private void TurnOnObject()
    {
        waveCollider.SetActive(true);
        cineCamera.SetActive(true);
    }
    private void SetCameraTrue()
    {
        cineCamera.SetActive(true);
    }

    private void SetCameraFalse()
    {
        cineCamera.SetActive(false);
    }

}

