using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{

    void Update()
    {
        transform.LookAt(GunController.instanse.transform);        
    }
}
