using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GunController : MonoBehaviour
{
    public Slider zoomValue;
    public float range = 10;
    public float damage = 50;
    public CinemachineVirtualCamera CMcamera;
    RaycastHit hit;
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    public void ScopeZoom()
    {
        CMcamera.m_Lens.FieldOfView = zoomValue.value;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            zoomValue.value -= Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            zoomValue.value += Time.deltaTime * 15;
        }
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                print("hit object " + hit.collider.name);
                if (hit.collider.CompareTag("Damageable"))
                {
                    if (hit.collider.name.Contains("Head"))
                    {
                        hit.transform.GetComponentInParent<IDamage>().TakeDamage(damage * 100,true);
                    }
                    else
                    {
                        hit.transform.GetComponentInParent<IDamage>().TakeDamage(damage,false);
                    }
                }
                // IDamage obj = hit.collider.GetComponent<IDamage>();
                // if (obj != null)
                // {
                //     obj.TakeDamage(damage);
                // }
            }
        }
    }
}
