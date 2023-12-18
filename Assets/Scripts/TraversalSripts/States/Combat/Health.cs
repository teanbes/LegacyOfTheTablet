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

    }

}