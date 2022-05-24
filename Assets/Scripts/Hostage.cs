using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Hostage : MonoBehaviour, IDamage
{
    public Slider powerBar;
    public bool isDie;
    public float health = 100;
    public NavMeshAgent agent;
    public Animator animator;
    public string runState, injureState, crwalState, dieState, hitState, hitAndFallState, currentState;
    private void Start()
    {
        powerBar.maxValue = health;
        powerBar.value = powerBar.maxValue;
    }
    public float walkRadius;
    private void Update()
    {
        if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            agent.SetDestination(GetRandomPoint());
            ChangeAnimationState(runState);
        }
       // MoveRandomPoint();
    }
    public void MoveRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        agent.destination = finalPosition;

    }
    public Vector3 GetRandomPoint()
    {
        NavMeshTriangulation navMashDate = NavMesh.CalculateTriangulation();
        int t = Random.Range(0, navMashDate.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMashDate.vertices[navMashDate.indices[t]], navMashDate.vertices[navMashDate.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMashDate.vertices[navMashDate.indices[t + 2]], Random.value);
        return point;
    }
    public void TakeDamage(float damage, bool isHead)
    {
        ChangeAnimationState(hitState);
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
        ChangeAnimationState(dieState);
        transform.eulerAngles = Vector3.right * 90;
        UIManager.instance.GameOver();
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
