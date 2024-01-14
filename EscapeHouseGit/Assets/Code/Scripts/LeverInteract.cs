using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverInteract : MonoBehaviour
{
    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource doorSound;

    public GameObject DoorBarn;
    DoorInteractController barnDoor;

    public AudioSource whispersSound;

    [SerializeField]
    public Transform noahDoor;

    public GameObject leverObject;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LeverTagInstance";
        barnDoor = DoorBarn.GetComponent<DoorInteractController>();

    }
    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;

            Vector3 rotationToAdd = new Vector3(90, 90, 90);
            transform.Rotate(rotationToAdd);

            barnDoor._isLocked = false;
            noahDoor.GetComponent<DoorInteractController>()._isLocked = false;
            whispersSound.enabled = true;

            doorSound.Play();
        }
    }

   
}
