using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FinalCutSceneScript : MonoBehaviour
{
    private PlayerInteractionsController _player = null;

    public GameObject interactTexts;
    public GameObject mainCamera;

    public bool triggered = false;

    public GameObject cutSceneCamera;

    public AudioSource lookHowSleepsSound;
    public GameObject lookHowSleepsCanvas;

    public AudioSource youKilledHerSound;
    public GameObject youKilledHerCanvas;

    public AudioSource dontSaySound;
    public GameObject dontSayCanvas;

    public AudioSource myFatherSound;
    public GameObject myFatherCanvas;

    public AudioSource yourMotherSound;
    public GameObject yourMotherCanvas;

    public AudioSource fireSound;
    public AudioSource fireSoundContinue;
    public GameObject fireObject;

    public GameObject playerControl;

    void OnTriggerEnter(Collider other)
    {
        if (triggered == false)
        {
            triggered = true;
            _player = FindObjectOfType<PlayerInteractionsController>();
            playerControl.SetActive(false);
            interactTexts.SetActive(false);

            StartCoroutine(PlaySubtitle());

            IEnumerator PlaySubtitle()
            {
                _player.enabled = false;
                mainCamera.SetActive(false);
                cutSceneCamera.SetActive(true);

                yield return new WaitForSeconds(1);

                lookHowSleepsSound.Play();
                lookHowSleepsCanvas.SetActive(true);
                yield return new WaitForSeconds(2);
                lookHowSleepsCanvas.SetActive(false);

                youKilledHerSound.Play();
                youKilledHerCanvas.SetActive(true);
                yield return new WaitForSeconds(1);
                youKilledHerCanvas.SetActive(false);

                dontSaySound.Play();
                dontSayCanvas.SetActive(true);
                yield return new WaitForSeconds(8);
                dontSayCanvas.SetActive(false);

                myFatherSound.Play();
                myFatherCanvas.SetActive(true);
                yield return new WaitForSeconds(4);
                myFatherCanvas.SetActive(false);

                yourMotherSound.Play();
                yourMotherCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                yourMotherCanvas.SetActive(false);

                fireSound.Play();
                yield return new WaitForSeconds(0.5f);
                fireObject.SetActive(true);
                fireSoundContinue.Play();
            }
        }
    }
}
