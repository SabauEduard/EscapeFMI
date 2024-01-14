using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerCubeSprinkler : MonoBehaviour
{
    [SerializeField]
    public Transform secondPuzzleDoor;

    private PlayerInteractionsController _player = null;

    public AudioSource playSound;
    public bool triggered = false;
    public GameObject textbox;
    public GameObject otherSound;
    public GameObject otherText;

    public GameObject DoorUniversity;
    DoorInteractController universityDoor;

    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        universityDoor = DoorUniversity.GetComponent<DoorInteractController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered == false)
        {
            PlayerInteractionsController.globalVariableForInteractionDesk += 1;
            if (PlayerInteractionsController.globalVariableForInteractionDesk == 2)
            {
                universityDoor._isLocked = false;
                secondPuzzleDoor.GetComponent<DoorInteractController>()._isLocked = false;
            }
            playSound.Play();
            otherSound.SetActive(false);
            otherText.SetActive(false);
            StartCoroutine(PlaySubtitle());
            triggered = true;
        }
    }

    IEnumerator PlaySubtitle()
    {
        textbox.SetActive(true);
        yield return new WaitForSeconds(5);
        textbox.SetActive(false);
    }
}
