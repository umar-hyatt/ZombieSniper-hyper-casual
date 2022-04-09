using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hostage : MonoBehaviour, IDamage
{
    public Slider powerBar;
    public bool isDie;
    public float health = 100;
    private void Start()
    {
        powerBar.maxValue = health;
        powerBar.value = powerBar.maxValue;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        powerBar.value = health;
        if (health <= 0)
        {
            isDie = true;
            Death();
        }
    }
    public void Death()
    {
        transform.eulerAngles = Vector3.right * 90;
        UIManager.instance.GameOver();
    }
}
