using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamage
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform target;
    public Slider powerBar;
    public Text headShotTxt;
    public float attackTime = 2f;
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
        animator = GetComponent<Animator>();
        currentHostage = target.GetComponent<Hostage>();
    }
    private void OnEnable()
    {
        isDie=false;
        powerBar.maxValue = health;
        powerBar.value = powerBar.maxValue;
        initPos = transform.position;
        CurrentTask += Chase;
    }
    private void OnDisable()
    {
        CurrentTask -= Chase;
        transform.position = initPos;
        ResetZombie();
    }
    private void Update()
    {
        if(isDie)return;
        AssignTask();
        CurrentTask.Invoke();
    }
    public bool isRun;
    public void AssignTask()
    {
        if (Vector3.Distance(target.position,transform.position)<=agent.stoppingDistance)
        {
            CurrentTask = Attack;
        }
        // else if (isDie)
        // {
        //     CurrentTask = Death;
        // }
        else
        {
            CurrentTask = Chase;
        }
    }
    public float walkSpeed,runSpeed;
    public void Chase()
    {
        if (!target) return;
        if(isRun)
        {  
            agent.speed=runSpeed;
            ChangeAnimationState(runState);
        }
        else
        {
            agent.speed=walkSpeed;
            ChangeAnimationState(walkState);
        }
        agent.SetDestination(target.position);
    }
    public Hostage currentHostage;
    public void Attack()
    {
        ChangeAnimationState(attackState);
    }

    public void TakeDamage(float damage,bool isHead)
    {
        if(isHead)
        {
            headShotTxt.gameObject.SetActive(true);
        }
        health -= damage;
        powerBar.value = health;
        if (health <= 0)
        {
            Death();
        }
    }
    public bool isDie;
    public void Death()
    {
        isDie = true;
        this.enabled = false;
        ChangeAnimationState(dieState);
      //  GetComponent<Renderer>().material.color = new Color(120f, 0f, 0f, 10f);
       // transform.localScale = Vector3.one * 0.2f;
    }
    public void GiveDamage()
    {
        print("give damage");
        currentHostage.TakeDamage(damage,false);
    }
    public string currentState, attackState, idleState, runState, dieState, walkState, injureState;
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    public void ResetZombie()
    {
        powerBar.value=powerBar.maxValue;
        health=powerBar.maxValue;
        headShotTxt.gameObject.SetActive(false);
    }
}
