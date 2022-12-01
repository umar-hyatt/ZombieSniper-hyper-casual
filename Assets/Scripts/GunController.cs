using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GunController : MonoBehaviour
{
    public static GunController instanse;
    private void Awake() {
        if(instanse==null)
        {
            instanse=this;
        }
    }
    public Slider zoomSlider;
    public float range = 10;
    public float damage = 50;
    public CinemachineVirtualCamera CMcamera;
    RaycastHit hit;
    public void ScopeZoom()
    {
        CMcamera.m_Lens.FieldOfView = zoomSlider.value;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            zoomSlider.value -= Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            zoomSlider.value += Time.deltaTime * 15;
        }
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                if (hit.collider.CompareTag("Damageable"))
                {
                    if (hit.collider.name.Contains("Head"))
                    {
                        print("hitted object " + hit.collider.name);
                        hit.transform.GetComponentInParent<IDamage>().TakeDamage(damage * 100,true);
                    }
                    else
                    {
                        print("hitted object " + hit.collider.name);
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
