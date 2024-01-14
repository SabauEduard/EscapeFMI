using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfInteractController : MonoBehaviour
{
    private PlayerInteractionsController _player = null;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private bool _alreadyMoved = false;
    private AudioSource audioSource;

    private int layerMask = ~(1 << 1);

    void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance, layerMask);

            if (cast && hit.collider.gameObject.GetComponent<SecretBookTag>())
                if (!_alreadyMoved)
                {
                    Debug.Log("Bookshelf moved");
                    _alreadyMoved = true;
                    MoveBookshelfSmoothly();
                }
        }
    }
    
    void MoveBookshelfSmoothly()
    {
        Vector3 firstMotion = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
        Vector3 secondMotion = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z - 0.5f);

        if(!audioSource.isPlaying)
            audioSource.Play();

        StartCoroutine(MoveToPosition(firstMotion, 1f)); 
        StartCoroutine(MoveToPosition(secondMotion, 1f, 1f)); // Adjust the duration and delay as needed
    }

    IEnumerator MoveToPosition(Vector3 target, float duration, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = target;
    }

}
