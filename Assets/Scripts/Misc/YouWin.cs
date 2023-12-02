using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{
    private readonly int FloatingHash = Animator.StringToHash("Floating");

    public PlayerStateMachine stateMachine;
    private const float CrossFadeDuration = 0.1f;
    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            
            stateMachine.Animator.CrossFadeInFixedTime(FloatingHash, CrossFadeDuration);
            AudioManager.Instance.Play("portal");
            Invoke("Delay", 3f);

        }


    }

    private void Delay()
    {
        SceneManager.LoadScene(3);
    }




}
