using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    [SerializeField] public GameObject hit;

    public void EnableHit()
    {
        hit.SetActive(true);
    }

    public void DisableHit()
    {
        hit.SetActive(false);
    }
}
