using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> destinations;
    public Animator animator;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, sightDistance, catchDistance, minChaseTime, maxChaseTime;
    public bool walking, chasing;
    public Transform player;

    private Transform _currentDest;
    private Vector3 _rayCastOffset;
    private float _distanceToPlayer;

    private void Start()
    {
        walking = true;
        _currentDest = destinations[Random.Range(0, destinations.Count)];
        _rayCastOffset = new Vector3(0, 1, 0);
    }

    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        _distanceToPlayer = Vector3.Distance(transform.position, player.position);
        RaycastHit hit;
        bool cast = Physics.Raycast(transform.position + _rayCastOffset, direction, out hit, sightDistance);
        Debug.DrawRay(transform.position + _rayCastOffset, direction * sightDistance, Color.red);
        if (hit.transform != null)
            Debug.Log(hit.transform.name);

        if (cast && hit.transform.GetComponent<PlayerTag>())
        {
            walking = false;
            StopCoroutine("Idle");
            StopCoroutine("Chase");
            StartCoroutine("Chase");
            chasing = true;
        }     
        if (chasing)
        {
            agent.destination = player.position;
            agent.speed = chaseSpeed;
            animator.ResetTrigger("walk");
            animator.ResetTrigger("idle");
            animator.SetTrigger("chase");
            if (_distanceToPlayer <= catchDistance)
            {
                animator.ResetTrigger("chase");
                death();
            }
        }
        if (walking)
        {
            agent.destination = _currentDest.position;
            agent.speed = walkSpeed;
            animator.ResetTrigger("idle");
            animator.ResetTrigger("chase");
            animator.SetTrigger("walk");
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.ResetTrigger("walk");
                animator.ResetTrigger("chase");
                animator.SetTrigger("idle");
                StopCoroutine("Idle");
                StartCoroutine("Idle");
                walking = false;
            }
           
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        walking = true;
        _currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(Random.Range(minChaseTime, maxChaseTime));
        stopChase();
    }

    public void stopChase()
    {
        walking = true;
        chasing = false;
        _currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    void death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //chasing = false;
        //walking = true;
        //_currentDest = destinations[Random.Range(0, destinations.Count)];
    }
}
