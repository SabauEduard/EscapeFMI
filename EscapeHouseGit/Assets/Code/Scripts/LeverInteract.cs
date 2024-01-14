using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverInteract : MonoBehaviour
{
    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource doorSound;

    public GameObject ghostStatue;
    public GameObject fireObject;

    public GameObject ghostCry;

    public GameObject houseMap;

    public GameObject DoorBarn;
    DoorInteractController barnDoor;

    public GameObject whispersSound;

    [SerializeField]
    public Transform noahDoor;

    public GameObject leverObject;

    [SerializeField]
    public float maxInteractDistance = 5.0f;

    private int layerMask = ~(1 << 1);

    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LeverTagInstance";
        barnDoor = DoorBarn.GetComponent<DoorInteractController>();

    }
    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance, layerMask);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;

            Vector3 rotationToAdd = new Vector3(90, 90, 90);
            transform.Rotate(rotationToAdd);

            StartCoroutine(PlaySubtitle());

            IEnumerator PlaySubtitle()
            {
                houseMap.SetActive(true);
                yield return new WaitForSeconds(1);
                fireObject.SetActive(true);
                fireObject.GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(4);
                ghostStatue.GetComponent<Animator>().enabled = true;
                yield return new WaitForSeconds(4);
                fireObject.GetComponent<Animator>().enabled = true;
                yield return new WaitForSeconds(5);
                fireObject.SetActive(false);
                ghostCry.SetActive(false);

                barnDoor._isLocked = false;
                doorSound.Play();
            }

            noahDoor.GetComponent<DoorInteractController>()._isLocked = false;
            whispersSound.SetActive(true);
        }
    }

   
}
