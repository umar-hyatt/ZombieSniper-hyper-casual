using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour, IDamage
{
    public Transform target;
    public Slider powerBar;
    public float attackTime=2f;
    public float speed;
    public float attackRange;
    public float damage;
    public float health;
    public delegate void Task();
    Task CurrentTask;
    Vector3 initPos;
    private void Start()
    {
        target = GameObject.FindWithTag("Hostage").transform;
        currentHostage=target.GetComponent<Hostage>();
    }
    private void OnEnable()
    {
        powerBar.maxValue=health;
        powerBar.value=powerBar.maxValue;
        initPos = transform.position;
        CurrentTask += Chase;
    }
    private void OnDisable()
    {
        CurrentTask -= Chase;
        transform.position = initPos;
    }
    private void Update()
    {
        if (CurrentTask == null) return;
        CurrentTask.Invoke();
    }
    public void Chase()
    {
        if (!target) return;
        if (Vector3.Distance(target.position, transform.position) <= attackRange)
        {
          StartCoroutine(Attack());
          CurrentTask=null;
          return;
        } 
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    public Hostage currentHostage;
    public IEnumerator Attack()
    {
        while (!currentHostage.isDie)
        {
            yield return new WaitForSeconds(attackTime);
            if(isDie){break;}
            currentHostage.TakeDamage(damage);
            if(currentHostage.isDie)
            {
                target=transform.parent;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        powerBar.value=health;
        if (health <= 0)
        {
            Death();
        }
    }
    public bool isDie;
    public void Death()
    {
        isDie=true;
        this.enabled=false;
        GetComponent<Renderer>().material.color = new Color(120f, 0f, 0f, 10f);
        transform.localScale = Vector3.one * 0.2f;
        CurrentTask -= Chase;
    }
}
