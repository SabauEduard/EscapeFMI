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
    public GameObject ghostInteract;

    public AudioSource doorSound;

    public GameObject DoorBarn;
    DoorInteractController barnDoor;


    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LetterTagDontSafe";
        barnDoor = DoorBarn.GetComponent<DoorInteractController>();
    }

    void Update()
    {

        if (!playSound.isPlaying)
        {
            RaycastHit hit;
            bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

            if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                boxCollider.enabled = false;

                PlayerInteractionsController.globalVariableForInteractionLetters += 1;
                if (PlayerInteractionsController.globalVariableForInteractionLetters == 1)
                {
                    doorSound.Play();
                    if (barnDoor._isOpen == false)
                    {
                        barnDoor._isLocked = true;
                    }
                    else
                    {
                        barnDoor.CloseDoor();
                        barnDoor._isLocked = true;
                    }
                }
                otherSound.Stop();
                otherText.SetActive(false);
                playSound.Play();
                StartCoroutine(PlaySubtitle());
                if (PlayerInteractionsController.globalVariableForInteractionLetters >= 2)
                {
                    ghostInteract.SetActive(true);
                }
            }
        }

        IEnumerator PlaySubtitle()
        {
            textbox.SetActive(true);
            yield return new WaitForSeconds(30);
            textbox.SetActive(false);
        }
    }
}
