using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{   // ilk önce damage alan objeyi kontrol et sonra 
    // hangi obje öldü onu kontrol et.
    private EnemyAnimator enemy_Anim;
    private EnemyController enemy_Controller;
    private NavMeshAgent navAgenT;
    public bool is_Player, is_Boar, is_Cannibal;
    private bool isDead;
    private float health = 100;
    private EnemyAudio enemy_Audio;
    private PlayerStats player_Stats;
    void Awake()
    {
        //if the health script boar or cannibal
        if(is_Boar || is_Cannibal)
        {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgenT = GetComponent<NavMeshAgent>();

            enemy_Audio = GetComponentInChildren<EnemyAudio>();

        }
        if (is_Player)
        {
            player_Stats = GetComponent<PlayerStats>();
        }
                
    }

    public void ApplyDamage(float damage)
    {
        if (isDead)
            return;

        health -= damage;
        if(is_Boar || is_Cannibal)
        {
            if (enemy_Controller.Enemy_State == EnemyState.PATROL)
            {
                enemy_Controller.chase_Distance = 50f;//to attack the player 
            }

        }
        if (is_Player)
        {
            //show the stats(display the health of player)
            player_Stats.DisplayHealthStats(health);
        }
        if (health <= 0f)
        {
            PlayerDied();

            isDead = true;

        }


    }//applyDamage;
    void PlayerDied()
    {
        if (is_Cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;



            enemy_Controller.enabled = false;
            navAgenT.enabled = false;
            enemy_Anim.enabled = false;
            //StartCoroutine
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(true);

        }
        if (is_Boar)
        {
            navAgenT.velocity = Vector3.zero;
            navAgenT.isStopped = true;
           
            enemy_Controller.enabled = false;
            enemy_Anim.Dead();

            //StartCoroutine
            StartCoroutine(DeadSound());
            //EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied(false);


        }
        if (is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            //Call enemy manager to stop spawning enemies
            EnemyManager.instance.StopSpawning();


            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            


        }
        //player dead
        if (gameObject.tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);

        }
        // boar or cannibal died (Enemy)
        else
        {
            Invoke("TurnOfGameObject", 3f);
        }




    }//player died.
    void RestartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    void TurnOfGameObject() {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemy_Audio.PlayDeadSound();


    }

}
