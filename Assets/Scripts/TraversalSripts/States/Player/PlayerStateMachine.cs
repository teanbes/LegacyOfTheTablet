using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public HealthBar HealthBar { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get;  set; }
    [field: SerializeField] public float RollMovementSpeed { get; set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public GameObject aura;
    [field: SerializeField] public GameObject DeathParticles { get; private set; }
    [field: SerializeField] public GameObject gameOverPanel;
    [field: SerializeField] public GameObject Sword;

    [field: SerializeField] public UIManager uiManager;

    public bool isSpell;
    public bool isWeapon;
    public bool isCombo;
    [HideInInspector] public bool isSaved;

    [HideInInspector] public bool RandomEnemiesArea1;
    [HideInInspector] public bool RandomEnemiesArea2;

    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    public Transform MainCameraTransform { get; private set; }

    [field: SerializeField] public Health Health;
    public int playerhealth;

    private void Start()
    {
        // Initial weapon state
        isWeapon = true;
        // Initial spell state
        isSpell = true;
        // Initial combo state
        isCombo = true;

        if (isWeapon) 
        {
            Sword.SetActive(true);
        }
        
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnIncreaseHealth += HandleHealthIncrease;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnIncreaseHealth -= HandleHealthIncrease;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        int currentHealth = Health.health;
        HealthBar.UpdateHealthBar(currentHealth);
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleHealthIncrease() 
    {
        // To do: add state
        //SwitchState(new PlayerGetsHealth(this)); 
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }
    
    public void DestroyObjectOnDeath()
    {
        Destroy(this);
    }
}
