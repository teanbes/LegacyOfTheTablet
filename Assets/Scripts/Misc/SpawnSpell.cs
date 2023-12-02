using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : MonoBehaviour
{
    public GameObject spell;
    public Transform spellSpawnPoint;


   
    public void ShootSpell()
    {
        Instantiate(spell, spellSpawnPoint.position, spellSpawnPoint.rotation);
    }


    
}
