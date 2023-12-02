using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffCollition : MonoBehaviour
{
    [SerializeField] public GameObject gameObject1;
    [SerializeField] public GameObject gameObject2;
    [SerializeField] public GameObject gameObject3;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("TurnOffObject", 2.3f);
            
        }


    }

    public void TurnOffObject()
    {
        if(gameObject)
            gameObject1.SetActive(false);
        if (gameObject2)
            gameObject2.SetActive(false);
        if (gameObject2)
            gameObject3.SetActive(false);
    }
}
