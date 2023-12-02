using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameInstance : MonoBehaviour
{

    public GameObject flameThrower;
    public Transform spawnPoint;
    public float spawnPointRotation;
    
    float time = 1f;

    private void Start()
    {
        
        spawnPoint.localRotation =Quaternion.Euler(0, spawnPointRotation, 0);

    }




    private void FInstance()
    {
        if( Time.time >= time ) 
        {
            time += 6f;
            Instantiate(flameThrower, spawnPoint.position, Quaternion.Euler(0, spawnPointRotation, 0));
        
            
        }


    }
    // Update is called once per frame
    void Update()
    {
        Invoke("FInstance", 6f);

    }
}
