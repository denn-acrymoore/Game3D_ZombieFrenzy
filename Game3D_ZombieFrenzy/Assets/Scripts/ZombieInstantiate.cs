using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInstantiate : MonoBehaviour
{
    public int enemyCount;
    public Transform[] spawnPoints;
    public GameObject zombiesPrefab;

    void Start()
    {
        SpawnNewZombies();

        StartCoroutine(EnemyDrop());
    }

    private void OnEnable()
    {
        ZombieBehavior.OnEnemyKilled += SpawnNewZombies;
    }

    void SpawnNewZombies()
    {
        int rand = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));
        Instantiate(zombiesPrefab, spawnPoints[rand].transform.position, Quaternion.identity);
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            Debug.Log("spawn yang baru" + enemyCount);
            SpawnNewZombies();
            yield return new WaitForSeconds(2f);
            enemyCount += 1;
        }
    }
}
