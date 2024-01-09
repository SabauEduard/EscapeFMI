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
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, sightDistance, catchDistance, minChaseTime, maxChaseTime, fovAngle, closeDistance, fovMultiplier;
    public bool walking, chasing;
    public Transform player;
    public AudioClip woodWalk, grassWalk, concreteWalk;
    private Transform _currentDest;
    private Transform _oldDest;
    private Vector3 _rayCastOffset;
    private float _distanceToPlayer;
    private Coroutine footstepCoroutine;
    private Coroutine chaseMusicCoroutine;
    [SerializeField]
    private AudioSource footstepAudioSource;
    [SerializeField]
    private AudioSource chaseMusicAudioSource;

    private void Start()
    {
        walking = true;
        updateDest();
        _rayCastOffset = new Vector3(0, 1, 0);
    }

    private void Update()
    {
        checkConditionsAndStartChase();
        if (chasing)
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                FootstepSoundCoroutine();
                ChaseMusicCoroutine();
            }

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
            if (agent.velocity.magnitude > 0.1f)
                FootstepSoundCoroutine();

            agent.destination = _currentDest.position;
            agent.speed = walkSpeed;
            animator.ResetTrigger("idle");
            animator.ResetTrigger("chase");
            animator.SetTrigger("walk");

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                if (!IsInvoking("StartIdle"))
                {
                    Invoke("StartIdle", 0.1f);
                }
            }
        }
    }

    void checkConditionsAndStartChase()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        _distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // if player is close to the enemy but not in fov, the enemy will still see the player with a multiplier depedning if it's moving or not
        float closeDistanceMultiplier = agent.velocity.magnitude > 0.1f ? 1f : 0.25f;
        // distance is weighted by fov angle as in the middle of the fov angle the distance is more important than at the edges
        float weightedDistance = _distanceToPlayer * Mathf.Lerp(1.0f, fovMultiplier, Mathf.InverseLerp(0.0f, fovAngle / 2.0f, angleToPlayer));
        weightedDistance = Mathf.Clamp(weightedDistance, sightDistance / 2.0f, sightDistance);

        if (angleToPlayer < fovAngle || weightedDistance < closeDistanceMultiplier * closeDistance)
        {
            RaycastHit hit;
            bool cast = Physics.Raycast(transform.position + _rayCastOffset, directionToPlayer, out hit, sightDistance);

            if (cast && hit.transform.GetComponent<PlayerTag>())
            {
                walking = false;
                chasing = true;
                StopCoroutine("Idle");
                StopCoroutine("Chase");
                StartCoroutine("Chase");
            }
        }
    }

    public bool shouldCatchIfGoingToHiding()
    {
        // function used by hiding place controller to check if the enemy should catch the player if he's going to a hiding place
        // (if the player is in Fov I use sightDistance instead of loseDistance from hiding place))
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        _distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(angleToPlayer < fovAngle)
        {
            RaycastHit hit;
            bool cast = Physics.Raycast(transform.position + _rayCastOffset, directionToPlayer, out hit, sightDistance);

            if (cast && hit.transform.GetComponent<PlayerTag>())
                return true;
        }
        return false;
    }

    void StartIdle()
    {
        StartCoroutine("Idle");
    }

    IEnumerator Idle()
    {
        walking = false;
        animator.ResetTrigger("walk");
        animator.ResetTrigger("chase");
        animator.SetTrigger("idle");

        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));

        walking = true;
        updateDest();
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
        updateDest();
    }

    void updateDest()
    {
        _currentDest = destinations[Random.Range(0, destinations.Count)];
        while (_currentDest == _oldDest)
        {
            _currentDest = destinations[Random.Range(0, destinations.Count)];
        }
        _oldDest = _currentDest;
    }

    void FootstepSoundCoroutine()
    {
        if (footstepCoroutine == null)
        {
            footstepCoroutine = StartCoroutine(PlayFootstepsWithDelay());
        }
    }

    void ChaseMusicCoroutine()
    {
        if (chaseMusicCoroutine == null)
        {
            chaseMusicCoroutine = StartCoroutine(PlayChaseMusic());
        }
    }

    IEnumerator PlayFootstepsWithDelay()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + _rayCastOffset, Vector3.down, out hit, 4.0f))
        {
            AudioClip footstepClip = null;
            
            if (hit.transform.GetComponent<WoodTag>())
            {
                footstepClip = woodWalk;
            }
            else if (hit.transform.GetComponent<GrassTag>())
            {
                footstepClip = grassWalk;
            }
            else if (hit.transform.GetComponent<ConcreteTag>())
            {
                footstepClip = concreteWalk;
            }
            

            if (footstepClip != null && footstepAudioSource != null)
            {
                footstepAudioSource.clip = footstepClip;        
                footstepAudioSource.Play();

                yield return new WaitForSeconds(footstepAudioSource.clip.length + Random.Range(0, 0.3f));
            }
        }


        footstepCoroutine = null;
    }

    IEnumerator PlayChaseMusic()
    {
        chaseMusicAudioSource.Play();

        while (chaseMusicAudioSource.isPlaying && chasing)
        {
            yield return null;
        }

        chaseMusicAudioSource.Stop();
        chaseMusicCoroutine = null;
    }


    void death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //chasing = false;
        //walking = true;
        //_currentDest = destinations[Random.Range(0, destinations.Count)];
    }
}
