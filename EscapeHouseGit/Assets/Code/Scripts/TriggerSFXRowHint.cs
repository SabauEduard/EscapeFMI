using System.Collections;
using UnityEngine;

public class TriggerSFXRowHint : MonoBehaviour
{
    public AudioSource playSound;
    public AudioSource doorOpenedSound;
    public bool triggered = false;
    public GameObject textbox;

    public GameObject DoorLibrary;
    DoorInteractController libraryDoor;

    private void Start()
    {
        libraryDoor = DoorLibrary.GetComponent<DoorInteractController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (triggered == false)
        {
            playSound.Play();
            doorOpenedSound.Play();
            libraryDoor._isLocked = false;
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
