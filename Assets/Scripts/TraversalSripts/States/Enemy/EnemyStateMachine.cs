using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public HealthBar HealthBar { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PatrolingSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackKnockback { get; private set; }
    [field: SerializeField] public GameObject DeathParticles { get; private set; }

    // Enemy Type: Follow or Patrol
    [field: SerializeField] public bool IsPatrolling { get; private set; }
    [field: SerializeField] public bool IsShooter { get; private set; }

    // Vars for anemy path
    [field: SerializeField] public Transform PathTarget;
    [field: SerializeField] public GameObject[] Path { get; private set; }
    [field: SerializeField] public int PathIndex;
    [field: SerializeField] public float DistThreshhold { get; private set; }

    public bool enemyDead = false;

    public Health player;

    private void Start()
    {
        // Get Components
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        // looks for empty gameobjects to set the path
        if (Path.Length <= 0) Path = GameObject.FindGameObjectsWithTag("Patrol");

        // Aproximation for path, almost never reaches 0 because of float calculations
        if (DistThreshhold <= 0) DistThreshhold = 0.5f;

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        if (!IsPatrolling)
            SwitchState(new EnemyIdleState(this));
        if (IsPatrolling)
            SwitchState(new EnemyPatrollingState(this));
        
    }
   
    private void GetPlayerComponents()
    {
        
    }
   
    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        int currentHealth = Health.health;
        HealthBar.UpdateHealthBar(currentHealth);
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
        enemyDead = true;   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }


}
