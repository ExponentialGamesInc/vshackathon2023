using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float startTime;
    public float duration;
    public int enemyCount;
    public GameObject Enemy;
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public List<Wave> waves = new List<Wave>();
    public float spawnDistance = 15;

    void Start()
    {
        foreach (var wave in waves)
        {
            StartCoroutine(SpawnWave(wave));
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        yield return new WaitForSeconds(wave.startTime);
        float spawnInterval = wave.duration / wave.enemyCount;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            Spawn(wave.Enemy);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void Spawn(GameObject enemy)
    {
        var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var dx = Mathf.Cos(angle);
        var dy = Mathf.Sin(angle);

        var newEnemy = Instantiate(enemy);
        newEnemy.transform.position = new Vector3(dx, dy, 0) * spawnDistance + player.transform.position;
    }
}
