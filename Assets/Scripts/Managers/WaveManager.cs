using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Health> enemiesInZone; // List to keep track of enemies in the zone
    [SerializeField] private Health miniBoss;
    [SerializeField] private bool isMiniBoss = false;
    [SerializeField] private Animator leftDoorAnimator;
    [SerializeField] private Animator rightDoorAnimator;// Animator for the combat animation
    private Health currentenemy;
    [SerializeField] private GameObject cineCamera;
    [SerializeField] private BoxCollider WaveTrigger;
    [SerializeField] private GameObject tabletOfDestinies;

    private void Start()
    {
        // Initialize the list of enemies in the zone

        foreach (Health enemy in enemiesInZone)
        {
            enemy.OnDie += HandleEnemyDeath; // Subscribe to the OnDeath event
        }

        miniBoss.OnDie += HandleMiniBoss;

    }

    // Method to handle enemy death
    private void HandleEnemyDeath()
    {
        int listSize = enemiesInZone.Count;
        enemiesInZone[listSize - 1] = currentenemy;

        enemiesInZone.Remove(currentenemy); // Remove the dead enemy from the list

        if (enemiesInZone.Count == 0 && !isMiniBoss)
        {
            // Invoke the SetCameraTrue method after 0.5 seconds
            Invoke("SetCameraTrue", 0.5f);

            Invoke("OpenDoors", 1.5f);

            // Invoke the SetCameraFalse method after 4.0 seconds to return to player camera
            Invoke("SetCameraFalse", 4.0f);
        }
        else if (enemiesInZone.Count == 0 && isMiniBoss)
        {
            miniBoss.gameObject.SetActive(true);
        }
    }

    private void HandleMiniBoss()
    {
        // Invoke the SetCameraTrue method after 0.5 seconds
        Invoke("SetCameraTrue", 0.5f);

        Invoke("OpenDoors", 1.5f);

        // Invoke the SetCameraFalse method after 4.0 seconds to return to player camera
        Invoke("SetCameraFalse", 4.0f);
    }

    private void SetCameraTrue()
    {
        cineCamera.SetActive(true);
    }

    private void SetCameraFalse()
    {
        cineCamera.SetActive(false);
        if (tabletOfDestinies)
        {
            tabletOfDestinies.GetComponent<BoxCollider>().isTrigger = true;
        }
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
