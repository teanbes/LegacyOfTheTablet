using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button continueButton;

    [Header("Pause Elements")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    private bool gamePaused;
    private PlayerStateMachine cRef;
    private EnemyStateMachine eRef;

    [Header("Audio Elements")]
    [SerializeField] private AudioSource backgroundMusic1;
    [SerializeField] private AudioSource backgroundMusic2;





    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 1 )
        {
            cRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();

        
            //Invoke("GetPlayerCOmponents", 1f);
            //eRef = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyStateMachine>();

        }

        if (playButton)
        {
            playButton.onClick.AddListener(StartGame);

        }


        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(BackToMainMenu);

        if (quitButton)
            quitButton.onClick.AddListener(GameQuit);

        if (continueButton)
            quitButton.onClick.AddListener(ContinueFromSave);

        if (!backgroundMusic1)
            Debug.Log("Please set Background music file 1");

        if (!backgroundMusic2)
            Debug.Log("Please set Background music file 2");




    }
    private void GetPlayerCOmponents()
    {
       //cRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();

        if (Input.GetKeyDown(KeyCode.M))
            LoadGame();


    }

    

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }

    public void ContinueFromSave()
    {

        SceneManager.LoadScene(1);
        
        Invoke("LoadGame", 1.5f);
       

    }


    public void BackToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        pausePanel.SetActive(gamePaused);

        if (gamePaused)
        {
            Time.timeScale = 0;
            PauseBackgorundMusic();
        }
            
        else
        {
            Time.timeScale = 1;
            UnpauseBackgorundMusic();

        }
    }

    public void SaveGame()
    {
        //cRef.SaveGamePrepare();
        //eRef.SaveGamePrepare();

    }

    public void LoadGame()
    {
        //cRef.LoadGameComplete();

    }

    public void PauseBackgorundMusic()
    {
        backgroundMusic1.Pause();
        backgroundMusic2.Pause();
    }

    public void UnpauseBackgorundMusic()
    {
        backgroundMusic1.UnPause();
        backgroundMusic2.UnPause();
    }



}
