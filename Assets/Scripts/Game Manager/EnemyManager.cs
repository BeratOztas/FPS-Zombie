using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField]
    private GameObject cannibal_Prefab, boar_Prefab;
    [SerializeField]
    private Transform[] cannibal_SpawnPoints, boar_SpawnPoints;
    [SerializeField]
    private int cannibal_Enemy_count, boar_Enemy_count;
    
    public float wait_Before_Spawn_Enemy=20f;
    private int initial_Cannibal_count,initial_Boar_count;
    void Awake()
    {
        MakeInstance();
    }
     void Start()
     {
        initial_Boar_count = boar_Enemy_count;
        initial_Cannibal_count = cannibal_Enemy_count;
        SpawnEnemies();
        StartCoroutine("CheckToSpawnEnemies");

     }
    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }
    // at first games start we spawn enemies
    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();

    }
    IEnumerator CheckToSpawnEnemies()
    {
        //we simply creating consistent coroutine
        yield return new WaitForSeconds(wait_Before_Spawn_Enemy);
        SpawnCannibals();
        SpawnBoars();
        StartCoroutine("CheckToSpawnEnemies");
    }
    void SpawnCannibals() {
        int index = 0;

        for(int i = 0; i < cannibal_Enemy_count; i++)
        {
            if (index >= cannibal_SpawnPoints.Length){
                index = 0;
            }
                
            Instantiate(cannibal_Prefab, cannibal_SpawnPoints[index].position, Quaternion.identity);
            index++;
            
        }
        cannibal_Enemy_count = 0;
    }
    void SpawnBoars() {
        int index = 0;
        for(int i = 0; i < boar_Enemy_count; i++)
        {
            if(index >= boar_SpawnPoints.Length){
                index = 0;
            }
            Instantiate(boar_Prefab, boar_SpawnPoints[index].position, Quaternion.identity);
            
        }
        boar_Enemy_count = 0;


    }
    public void EnemyDied(bool isCannibal)
    {
        if (isCannibal)
        {
            cannibal_Enemy_count++;
            if (cannibal_Enemy_count > initial_Cannibal_count)
                cannibal_Enemy_count = initial_Cannibal_count;

        }
        else
        {
            boar_Enemy_count++;
            if (boar_Enemy_count > initial_Boar_count)
                boar_Enemy_count = initial_Boar_count;
        
        }
    }
    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }






}
