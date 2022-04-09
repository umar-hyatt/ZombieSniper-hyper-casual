using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamage
{
    public Transform target;
    public Animator animator;
    public NavMeshAgent agent;
    public string attack,walk,injure,run,idle,die,floorBitting,bitting,currentState;
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
        CurrentTask?.Invoke();
    }
    public void Chase()
    {
        if (!target) return;
        ChangeAnimationState(walk);
        if (agent.pathPending&&agent.path.status!=NavMeshPathStatus.PathComplete)
        {
          StartCoroutine(Attack());
          CurrentTask=null;
          return;
        } 
       agent.SetDestination(target.transform.position);
    }
    public Hostage currentHostage;
    public IEnumerator Attack()
    {
        while (!currentHostage.isDie)
        {
            yield return new WaitForSeconds(attackTime);
            if(isDie){break;}
            ChangeAnimationState(attack);
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
    void ChangeAnimationState(string newState)
    {
        if(currentState==newState)return;
        animator.Play(newState);
        currentState=newState;
    }
}
