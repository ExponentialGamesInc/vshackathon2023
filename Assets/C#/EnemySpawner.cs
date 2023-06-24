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
    public int spawned;
    public float lastStep;
}
public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    float gameStart;
    public GameObject player;
    public List<Wave> waves = new List<Wave>();
    public float spawnDistance = 15;
    void Start()
    {
        float gameStart = Time.realtimeSinceStartup;
        foreach (var wave in waves)
        {
            wave.spawned = wave.enemyCount;
            wave.lastStep = wave.startTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float gameTime = Time.realtimeSinceStartup - gameStart;
        foreach (var wave in waves)
        {
            if (wave.startTime < gameTime && wave.startTime + wave.duration > gameTime)
            {
                var delT = gameTime - wave.lastStep;
                var enemiesPerSecond = wave.enemyCount / wave.duration;

                if (wave.spawned > 0)
                {
                    var enemiesToSpawn = Mathf.RoundToInt(delT * enemiesPerSecond);
                    for (int i = 0; i < enemiesToSpawn; i++)
                    {
                        spawn(wave.Enemy);
                        wave.spawned -= 1;
                    }
                    wave.lastStep += enemiesToSpawn/enemiesPerSecond;
                }
                else if (wave.spawned > 0 && gameTime > wave.startTime + wave.duration)
                {
                    for (int i = 0; i < wave.spawned; i++)
                    {
                        spawn(wave.Enemy);
                    }
                }
            }
        }
    }

    public void spawn(GameObject enemy)
    {
        var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        var dx = Mathf.Cos(angle);
        var dy = Mathf.Sin(angle);

        var newEnemy = Instantiate(enemy);
        newEnemy.transform.position = new Vector3(dx, dy, 0) * spawnDistance + player.transform.position;
    }
}
