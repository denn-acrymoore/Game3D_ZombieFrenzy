using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInstantiate : MonoBehaviour
{
    public int enemyCount;
    public Transform[] spawnPoints;
    public GameObject zombiesPrefab;
    
    private float spawnXOffset = 1f;
    private float spawnZOffset = 1f;

    void Start()
    {
        SpawnNewZombies();

        StartCoroutine(EnemyDrop());
    }

    private void OnEnable()
    {
        ZombieBehavior.OnEnemyKilled += SpawnNewZombies;
        ZombieBehavior.OnEnemyKilled += ReduceZombieAmount;
    }

    void SpawnNewZombies()
    {
        enemyCount += 1;
        //Debug.Log("spawn yang baru " + enemyCount);

        int rand = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));

        float randXOffset = Random.Range(-spawnXOffset, spawnXOffset);
        float randZOffset = Random.Range(-spawnZOffset, spawnZOffset);

        Vector3 pos = spawnPoints[rand].transform.position 
            + Vector3.right * randXOffset 
            + Vector3.forward * randZOffset;

        Instantiate(zombiesPrefab, pos, Quaternion.identity);
    }

    void ReduceZombieAmount()
    {
        --enemyCount;
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 5)
        {
            SpawnNewZombies();
            yield return new WaitForSeconds(5f);
        }
    }
}
