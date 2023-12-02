using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GolemHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] public int health;
   
    public event Action OnTakeDamage;
    public event Action OnDie;

   

    public bool IsDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

  
    public void DealDamage(int damage)
    {

        if (health == 0) { return; }

        health = Mathf.Max(health - damage, 0);

        OnTakeDamage?.Invoke();


        Debug.Log(health);

     
        if (health == 0)
        {
            OnDie?.Invoke();

        }

    }



}
