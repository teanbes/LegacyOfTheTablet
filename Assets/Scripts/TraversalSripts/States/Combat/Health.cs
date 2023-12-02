using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [Header("Health Bar Sprites")]
    public RawImage[] healthBar;

    public int health;
    private bool isInvulnerable;

    public event Action OnTakeDamage;
    public event Action OnIncreaseHealth;
    public event Action OnDie;



    public bool IsDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
       
        if (health == 0) { return; }

        if (isInvulnerable) { return; }

        health = Mathf.Max(health - damage, 0);

        OnTakeDamage?.Invoke();


        Debug.Log(health);

        HealthBarHandler();


        if (health == 0)
        {
            OnDie?.Invoke();

        }

    }

    public void IncreaseHealth(int healthUp)
    {
        if (health == 0) { return; }

        if (isInvulnerable) { return; }

        health = Mathf.Max(health + healthUp, 0);

        if (health >= 100) { health = 100; }

        OnIncreaseHealth?.Invoke();

        Debug.Log("player health" + health);

        HealthBarHandler();
    }

    public void HealthBarHandler()
    {
        if (healthBar.Length>0)
        {

            switch (health)
            {
                case 100:
                    healthBar[0].enabled = true;
                    healthBar[1].enabled = true;
                    healthBar[2].enabled = true;
                    healthBar[3].enabled = true;
                    healthBar[4].enabled = true;
                    break;

                case 80:
                    healthBar[0].enabled = false;
                    healthBar[1].enabled = true;
                    healthBar[2].enabled = true;
                    healthBar[3].enabled = true;
                    healthBar[4].enabled = true;
                    break;

                case 60:
                    healthBar[0].enabled = false;
                    healthBar[1].enabled = false;
                    healthBar[2].enabled = true;
                    healthBar[3].enabled = true;
                    healthBar[4].enabled = true;
                    break;

                case 40:
                    healthBar[0].enabled = false;
                    healthBar[1].enabled = false;
                    healthBar[2].enabled = false;
                    healthBar[3].enabled = true;
                    healthBar[4].enabled = true;
                    break;

                case 20:
                    healthBar[0].enabled = false;
                    healthBar[1].enabled = false;
                    healthBar[2].enabled = false;
                    healthBar[3].enabled = false;
                    healthBar[4].enabled = true;
                    break;

                case 0:
                    healthBar[0].enabled = false;
                    healthBar[1].enabled = false;
                    healthBar[2].enabled = false;
                    healthBar[3].enabled = false;
                    healthBar[4].enabled = false;

                    break;

            }
        }
    }

 
}