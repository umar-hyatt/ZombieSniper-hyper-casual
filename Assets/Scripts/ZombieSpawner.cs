using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public static ZombieSpawner instance;
    void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
    }
    public GameObject zombiePrefab;
    BoxCollider objectCollider;
    private void Start()
    {
        objectCollider = GetComponent<BoxCollider>();
    }
    public IEnumerator SpawnObject()
    {
        while (0 < WaveData.zombieStoreEachPoint)
        {
        print("spawn objects");
            WaveData.zombieStoreEachPoint--;
            WaveData.totalZombie--;
            yield return new WaitForSeconds(WaveData.spawnDely);
            for (int i = 1; i <= WaveData.objSpawnEachTime; i++)
            {
                PoolZombie().SetActive(true);
            }
        }
    }
    public Vector3 RandomPointInBounds()
    {
        return new Vector3(
            Random.Range(objectCollider.bounds.min.x, objectCollider.bounds.max.x),
            transform.position.y,
            Random.Range(objectCollider.bounds.min.z, objectCollider.bounds.max.z)
        );
    }
    public GameObject PoolZombie()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return transform.GetChild(i).gameObject;
            }
        }
        var obj = Instantiate(zombiePrefab, RandomPointInBounds(), Quaternion.identity);
        obj.transform.parent = transform;
        return obj;
    }
}
