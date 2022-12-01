using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _zombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hostage")
        {
            zombie.SetActive(true);
        }
    }
}
