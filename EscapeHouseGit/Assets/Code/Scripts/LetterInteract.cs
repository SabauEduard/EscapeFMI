using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInteract : MonoBehaviour
{
    [SerializeField]
    public Transform secondPuzzleDoor;
    public List<int> _usedKeys = new List<int>();

    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;

    public AudioSource playSound;
    public GameObject textbox;

    public AudioSource otherSound;
    public GameObject otherText;

    public GameObject DoorUniversity;
    DoorInteractController universityDoor;

    public GameObject sprinklerHintTrigger;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "LetterTag";
        universityDoor = DoorUniversity.GetComponent<DoorInteractController>();
    }

    void Update()
    {
        if (!playSound.isPlaying)
        {
            RaycastHit hit;
            bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

            if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
            {
                PlayerInteractionsController.globalVariableForInteractionDesk += 1;
                if (PlayerInteractionsController.globalVariableForInteractionDesk == 2)
                {
                    universityDoor._isLocked = false;
                    secondPuzzleDoor.GetComponent<DoorInteractController>()._isLocked = false;
                }
                if (otherSound.isPlaying)
                {
                    otherSound.enabled = false;
                    otherText.SetActive(false);
                }
                playSound.Play();
                StartCoroutine(PlaySubtitle());
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                boxCollider.enabled = false;
            }
        }

        IEnumerator PlaySubtitle()
        {
            textbox.SetActive(true);
            yield return new WaitForSeconds(30);
            textbox.SetActive(false);
            sprinklerHintTrigger.SetActive(true);
        }
    }
}
