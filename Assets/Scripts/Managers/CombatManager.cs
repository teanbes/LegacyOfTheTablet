using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<Health> enemiesInZone; // List to keep track of enemies in the zone
    public Animator combatAnimator; // Animator for the combat animation
    private Health currentenemy;

    private void Start()
    {
        // Initialize the list of enemies in the zone
        enemiesInZone = new List<Health>();

        // Add all enemies in the zone to the list
        Health[] enemies = GetComponentsInChildren<Health>();
        foreach (Health enemy in enemies)
        {
            enemiesInZone.Add(enemy);

            enemy.OnDie += HandleEnemyDeath; // Subscribe to the OnDeath event
            currentenemy = enemy;

        }
    }

    // Method to handle enemy death
    private void HandleEnemyDeath()
    {
        enemiesInZone.Remove(currentenemy); // Remove the dead enemy from the list

        if (enemiesInZone.Count == 0)
        {
            // All enemies are dead, trigger combat animation
            //combatAnimator.SetTrigger("CombatEnd");
            Debug.Log("todos muertos");
        }
    }
}
