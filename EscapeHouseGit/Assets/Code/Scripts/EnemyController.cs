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
        Vector3 direction = (player.position - transform.position).normalized;
        _distanceToPlayer = Vector3.Distance(transform.position, player.position);
        RaycastHit hit;
        bool cast = Physics.Raycast(transform.position + _rayCastOffset, direction, out hit, sightDistance);

        if (cast && hit.transform.GetComponent<PlayerTag>())
        {
            walking = false;
            chasing = true;
            StopCoroutine("Idle");
            StopCoroutine("Chase");
            StartCoroutine("Chase");
        }     
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
