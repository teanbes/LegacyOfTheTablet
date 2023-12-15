using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Health> enemiesInZone; // List to keep track of enemies in the zone
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;// Animator for the combat animation
    private Health currentenemy;
    [SerializeField] private GameObject cineCamera;
    [SerializeField] private BoxCollider WaveTrigger;

    private void Start()
    {
        // Initialize the list of enemies in the zone

        foreach (Health enemy in enemiesInZone)
        {
            enemy.OnDie += HandleEnemyDeath; // Subscribe to the OnDeath event
        }
    }

    // Method to handle enemy death
    private void HandleEnemyDeath()
    {
        int listSize = enemiesInZone.Count;
        enemiesInZone[listSize - 1] = currentenemy;

        enemiesInZone.Remove(currentenemy); // Remove the dead enemy from the list

        if (enemiesInZone.Count == 0)
        {
            // Invoke the SetCameraTrue method after 0.5 seconds
            Invoke("SetCameraTrue", 0.5f);

            Invoke("OpenDoors", 1.5f);

            // Invoke the SetCameraFalse method after 4.0 seconds to return to player camera
            Invoke("SetCameraFalse", 4.0f);
        }
    }

    private void SetCameraTrue()
    {
        cineCamera.SetActive(true);
    }

    private void SetCameraFalse()
    {
        cineCamera.SetActive(false);
    }

    private void OpenDoors()
    {
        if (leftDoorAnimator)
            leftDoorAnimator.SetTrigger("Open");
        if (rightDoorAnimator)
            rightDoorAnimator.SetTrigger("Open");
        //WaveTrigger.enabled = false;
    }

}
