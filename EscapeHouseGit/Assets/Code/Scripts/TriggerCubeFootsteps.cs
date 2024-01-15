using System.Collections;
using UnityEngine;

public class TriggerCubeFootsteps : MonoBehaviour
{
    public AudioSource playSound;
    public bool triggered = false;
    public GameObject textbox;
    public GameObject otherSound;
    public GameObject otherText;


    void OnTriggerEnter(Collider other)
    {
        if (triggered == false)
        {
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
