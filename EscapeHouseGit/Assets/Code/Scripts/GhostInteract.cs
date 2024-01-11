using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostInteract : MonoBehaviour
{
    public List<int> _usedKeys = new List<int>();

    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource leavemeAloneSound;
    public AudioSource helpSound;
    public AudioSource EitherYouSound;
    public AudioSource SeemsLikeSound;
    public GameObject flashback;
    public GameObject leavemeAlone;
    public GameObject help;
    public GameObject EitherYou;
    public GameObject SeemsLike;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "GhostTag";
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            StartCoroutine(PlaySubtitle());
        }

        IEnumerator PlaySubtitle()
        {
            flashback.SetActive(true);
            yield return new WaitForSeconds(3);
            flashback.SetActive(false);

            yield return new WaitForSeconds(1);

            leavemeAloneSound.Play();
            leavemeAlone.SetActive(true);
            yield return new WaitForSeconds(4);
            leavemeAlone.SetActive(false);

            helpSound.Play();
            help.SetActive(true);
            yield return new WaitForSeconds(2);
            help.SetActive(false);

            EitherYouSound.Play();
            EitherYou.SetActive(true);
            yield return new WaitForSeconds(5);
            EitherYou.SetActive(false);

            SeemsLikeSound.Play();
            SeemsLike.SetActive(true);
            yield return new WaitForSeconds(17);
            SeemsLike.SetActive(false);
        }
    }
}
