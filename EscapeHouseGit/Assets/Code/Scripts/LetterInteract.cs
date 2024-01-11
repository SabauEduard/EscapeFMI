using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInteract : MonoBehaviour
{
    public List<int> _usedKeys = new List<int>();

    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource playSound;
    public GameObject textbox;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LetterTag";
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            playSound.Play();
            StartCoroutine(PlaySubtitle());
        }

        IEnumerator PlaySubtitle()
        {
            textbox.SetActive(true);
            yield return new WaitForSeconds(30);
            textbox.SetActive(false);
        }
    }
}
