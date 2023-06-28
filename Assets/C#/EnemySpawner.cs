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

    public Wave(float startTime, float duration, int enemyCount, GameObject Enemy)
    {
        this.startTime = startTime;
        this.duration = duration;
        this.enemyCount = enemyCount;
        this.Enemy = Enemy;
    }
}


public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public List<Wave> waves = new List<Wave>();
    public float spawnDistance = 15;
    public List<GameObject> enemies = new List<GameObject>();
    public Dictionary<GameObject, int> map = new Dictionary<GameObject, int>();
    public float currentTime = 0;
    public int currentDiff = 0;

    void Start()
    {
        map.Add(enemies[0], 1);
        map.Add(enemies[1], 5);
        map.Add(enemies[2], 25);

        waves.Clear();
        currentDiff = 10;

        for (int i = 0; i < 200; i++)
        {
            var random = Random.Range(1, 5);

            Wave newWave = GenerateWave(currentDiff / random, (int)currentTime);
            waves.Add(newWave);
            newWave = GenerateWave(currentDiff - (currentDiff / random), (int)currentTime + Random.Range(1, 6));
            waves.Add(newWave);
            currentTime += newWave.duration + (int)Random.Range(2, 4) * 5;
            currentDiff += 5 + i;
        }

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

    public Wave GenerateWave(int d, int t)
    {
        int r = 0;
        if (d == 0)
            return new Wave(0, 0, 0, enemies[0]);

        while (true)
        {
            r = Random.Range(0, 3);
            print(map[enemies[r]] + ", " + d);
            if (map[enemies[r]] < d)
                break;
        }

        return new Wave(t, Random.Range(2, 5) * 5f, Mathf.RoundToInt((float)d/ map[enemies[r]]), enemies[r]);
    }
}
