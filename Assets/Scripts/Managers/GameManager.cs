using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject welcomePanel;
    private bool isPaused;

    [HideInInspector] public bool playerIsDead;

    private static GameManager _instance = null;

    // Reference to single active instance of object - for singleton behaviour
    public static GameManager Instance
    {
        get => _instance;
    }


    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerIsDead = false;
        //Time.timeScale = 0.0f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.Play("Select");
            PauseGame();
        }
    }


    public PlayerStateMachine playerPrefab;
    [HideInInspector] public PlayerStateMachine playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint;


    public void SpawnPlayer(Transform spawnPoint)
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        currentSpawnPoint = spawnPoint; 
    }

    public void Respawn()
    {

        if (playerInstance)
            playerInstance.transform.position = currentSpawnPoint.position;
    }

    public void TestingGrounds()
    {
        welcomePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Quit() 
    {
            Application.Quit();

    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            AudioManager.Instance.Play("Select");
            Time.timeScale = 1;
        }
    }
} 
