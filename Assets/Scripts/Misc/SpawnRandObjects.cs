using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandObjects : MonoBehaviour
{
    //public List<GameObject> collectibles;
    public GameObject[] collectibles;
    Vector3 currentEulerAngles;
    Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPowerUps(collectibles);


    }
    
    public void SpawnPowerUps(GameObject[] collectibles)
    {
        if (collectibles.Length > 0)
        {
            currentEulerAngles = new Vector3(-90, 0, 0);
            currentRotation.eulerAngles = currentEulerAngles;
            transform.rotation = currentRotation;
            int index = Random.Range(0, collectibles.Length);


            Instantiate(collectibles[index], transform.position, transform.rotation);

        }

    }

}
