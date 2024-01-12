using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterInteractDontSafe : MonoBehaviour
{
    public List<int> _usedKeys = new List<int>();

    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource playSound;
    public AudioSource otherSound;
    public GameObject textbox;
    public GameObject otherText;


    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LetterTagDontSafe";
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            otherSound.Stop();
            otherText.SetActive(false);
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
