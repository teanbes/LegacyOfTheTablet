using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<Health> enemiesInZone; // List to keep track of enemies in the zone
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;// Animator for the combat animation
    private Health currentenemy;
    [SerializeField] private GameObject cineCamera;

    private void Start()
    {
        // Initialize the list of enemies in the zone
        
        foreach (Health enemy in enemiesInZone)
        {
            currentenemy = enemy;
            enemy.OnDie += HandleEnemyDeath; // Subscribe to the OnDeath event
            Debug.Log($"Enemy Name: {enemy.name}");
        }
    }

    // Method to handle enemy death
    private void HandleEnemyDeath()
    {
        enemiesInZone.Remove(currentenemy); // Remove the dead enemy from the list

        if (enemiesInZone.Count == 0)
        {
            // All enemies are dead, trigger combat animation
            leftDoorAnimator.SetTrigger("Open");
            rightDoorAnimator.SetTrigger("Open");
            Debug.Log("todos muertos");

            // Invoke the SetCameraTrue method after 0.5 seconds
            Invoke("SetCameraTrue", 0.5f);

            // Invoke the SetCameraFalse method after 1.5 seconds
            Invoke("SetCameraFalse", 4.0f);
        }
    }

    private void SetCameraTrue()
    {
         // Enable the camera
         cineCamera.SetActive(true);
    }

    // Method to set the camera to false
    private void SetCameraFalse()
    {
        cineCamera.SetActive(false);
    }

}
