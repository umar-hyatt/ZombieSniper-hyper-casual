using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float range = 10;
    public float damage = 50;
    RaycastHit hit;
    private void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                IDamage obj = hit.collider.GetComponent<IDamage>();
                if (obj != null)
                {
                    obj.TakeDamage(damage);
                }
            }
        }
    }
}
