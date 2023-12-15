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
    [SerializeField] private Button resumeButton;

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
        if (playButton)
            playButton.onClick.AddListener(StartGame);


        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(BackToMainMenu);

        if (quitButton)
            quitButton.onClick.AddListener(GameQuit);

        if (resumeButton)
            resumeButton.onClick.AddListener(Resume);

        if (!backgroundMusic1)
            Debug.Log("Please set Background music file 1");

        if (!backgroundMusic2)
            Debug.Log("Please set Background music file 2");
    }

  
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.P))
                PauseGame();
        }

    }

    

    public void StartGame()
    {
        SceneManager.LoadScene("Graveyard");
    }

    public void Resume()
    {
        PauseGame();
    }


    public void BackToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        AudioManager.Instance.Play("Select");
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

    public void PauseBackgorundMusic()
    {
        backgroundMusic1.Pause();
    }

    public void UnpauseBackgorundMusic()
    {
        backgroundMusic1.UnPause();
    }



}
