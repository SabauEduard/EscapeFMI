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

    public GameObject leverObject;

    public float sliderLastX = -45.0f;

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
            Vector3 rotationToAdd = new Vector3(90, 90, 90);
            transform.Rotate(rotationToAdd * Time.deltaTime);
            barnDoor._isLocked = false;
            barnDoor._isOpen = true;
            doorSound.Play();
        }
    }

   
}
