using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public ZombieSpawner[] zombieSpawners;
    private void Start()
    {
        print("wave spawner cs gameObj name" + gameObject.name);
        zombieSpawners=FindObjectsOfType<ZombieSpawner>();
        StartCoroutine(SpawnStart());
    }
    private IEnumerator SpawnStart()
    {
        yield return new WaitForSeconds(3);
        WaveData.zombieStoreEachPoint = WaveData.totalZombie / zombieSpawners.Length;
        foreach (var item in zombieSpawners)
        {
            StartCoroutine(item.SpawnObject());
        }
    }
    public void WaveEnd()
    {
        WaveData.waveNumber++;
        StartCoroutine(SpawnStart());
    }
}
public static class WaveData
{
    public static int waveNumber = 1;
    public static int totalZombie = 10;
    public static float spawnDely = 3;
    public static int objSpawnEachTime = 1;
    public static int zombieStoreEachPoint;
}
