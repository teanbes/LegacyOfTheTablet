using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStateMachine>())
        {
            Invoke("CloseDoor", 0.1f);
            Destroy(gameObject, 1.8f);
        }
    }

    private void CloseDoor()
    {
        doorAnimator.SetTrigger("Close");
    }

}
