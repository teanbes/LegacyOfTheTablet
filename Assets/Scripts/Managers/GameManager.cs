using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool playerIsDead;
    private static GameManager _instance = null;

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
    }

    public PlayerStateMachine playerPrefab;
    [HideInInspector] public PlayerStateMachine playerInstance = null;
    [HideInInspector] public Transform currentSpawnPoint;

} 
