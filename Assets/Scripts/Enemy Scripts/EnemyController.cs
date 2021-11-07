using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}


public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemy_anim;
    private EnemyState enemy_State;
    private NavMeshAgent navAgent;
    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 15f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;
    public GameObject enemy_Attack_Point;

    private EnemyAudio enemy_Audio;



     void Awake()
    {
        enemy_anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
        enemy_Audio = GetComponentInChildren<EnemyAudio>();
        
    }
    void Start()
    {
        // Enemy first gonna patrol
        enemy_State = EnemyState.PATROL;

        //as soon as game starts we need to set patrol timer 
        //cause enemy to patrol
        patrol_Timer = patrol_For_This_Time;

        //when the enemy first gets to the play
        //attack right away  enemy are gonna wait a little bit before attack the player
        attack_Timer = wait_Before_Attack;

        //memorize the value of chase distance
        //so that we can put it back
        current_Chase_Distance = chase_Distance;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if(enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }

    }
    void TurnOnEnemyAttackPoint() {
        enemy_Attack_Point.SetActive(true);
    
    }
    void TurnOffEnemyAttackPoint() {
        if (enemy_Attack_Point.activeInHierarchy)
        {
            enemy_Attack_Point.SetActive(false);
        }
    
    }
    void Patrol() {
        //enable the agent move again.
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;
        //add to the patrol timer
        patrol_Timer += Time.deltaTime;

        if (patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }
        //if we are moving
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_anim.Walk(true);

        }
        else
        {
            enemy_anim.Walk(false);
        }
        //test the distance between enemy and player
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_anim.Walk(false);
            enemy_State = EnemyState.CHASE;
            //play spotted audio
            enemy_Audio.PlayScreamSound();

        }
    
    }//patrol
    void Chase() {
        //enable the agent move again.
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;
        // enemy are gonna chase player.
        navAgent.SetDestination(target.position);
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_anim.Run(true);

        }
        else
        {
            enemy_anim.Run(false);
        }
        if(Vector3.Distance(transform.position,target.position) <= attack_Distance)
        {
            //stop the animations.
            enemy_anim.Run(false);
            enemy_anim.Walk(false);
            enemy_State = EnemyState.ATTACK;
            //reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance)
                chase_Distance = current_Chase_Distance;
        }
        //we are running from enemy
        else if (Vector3.Distance(transform.position, target.position) > chase_Distance)
        {
            enemy_anim.Run(false);
            enemy_State = EnemyState.PATROL;

            //reset the patrol timer so that function
            //can calculate  the new patrol destination right away
            patrol_Timer = patrol_For_This_Time;
            //reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance)
                chase_Distance = current_Chase_Distance;

        }

    }//chase
    void Attack() {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack)
        {
            enemy_anim.Attack();
            attack_Timer = 0f;
            //player attack sound
            enemy_Audio.PlayAttackSound();
        }
        if (Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }

    }//attack

    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);
        navAgent.SetDestination(navHit.position);

    }
    public EnemyState Enemy_State
    {
        get; set;
    }



}//class






