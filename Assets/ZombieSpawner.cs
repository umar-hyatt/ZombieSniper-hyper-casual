using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float spawnTime;
    public int storedObject;
    public GameObject zombiePrefab;
    private void Start()
    {
        StartCoroutine(SpawnObject());
    }
    IEnumerator SpawnObject()
    {
        while (0 < storedObject)
        {
            storedObject--;
            yield return new WaitForSeconds(spawnTime);
            PoolZombie().SetActive(true);
        }
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
        var obj = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        return obj;
    }
}
