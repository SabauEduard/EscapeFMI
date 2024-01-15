using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutSceneScript : MonoBehaviour
{
    private PlayerInteractionsController _player = null;

    public GameObject interactTexts;
    public GameObject mainCamera;

    public bool triggered = false;

    public GameObject cutSceneCamera;
    public GameObject finalCutSceneCamera;

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

    public GameObject newspaper;

    public AudioSource breakingNewsSound;
    public GameObject breakingNewsCanvas;

    public AudioSource side_breakingNewsSound;

    public AudioSource onARainyAfternoonSound;
    public GameObject onARainyAfternoonCanvas;

    public AudioSource side_onARainyAfternoonSound;

    public AudioSource theCareTakersSound;
    public GameObject theCareTakersCanvas;

    public AudioSource side_theMisteryAtFMISound;
    public GameObject side_theMisteryAtFMICanvas;

    public AudioSource theTragedyClaimedSound;
    public GameObject theTragedyClaimedCanvas;

    public AudioSource WeWillContinueSound;
    public GameObject WeWillContinueCanvas;

    public AudioSource TypingSound;

    public GameObject CreditCanvas;

    public GameObject playerControl;

    public GameObject whisperSound;

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
                newspaper.SetActive(true);
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

                yield return new WaitForSeconds(4);

                fireSound.Stop();
                fireSoundContinue.Stop();

                cutSceneCamera.SetActive(false);
                finalCutSceneCamera.SetActive(true);
                finalCutSceneCamera.GetComponent<Animator>().enabled = true;

                yield return new WaitForSeconds(1);

                TypingSound.Play();

                breakingNewsSound.Play();
                breakingNewsCanvas.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                side_breakingNewsSound.Play();
                yield return new WaitForSeconds(2.5f);
                breakingNewsCanvas.SetActive(false);

                side_theMisteryAtFMISound.Play();
                side_theMisteryAtFMICanvas.SetActive(true);
                yield return new WaitForSeconds(4);
                side_theMisteryAtFMICanvas.SetActive(false);

                onARainyAfternoonSound.Play();
                onARainyAfternoonCanvas.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                side_onARainyAfternoonSound.Play();
                yield return new WaitForSeconds(16);
                onARainyAfternoonCanvas.SetActive(false);

                theCareTakersSound.Play();
                theCareTakersCanvas.SetActive(true);
                yield return new WaitForSeconds(27);
                theCareTakersCanvas.SetActive(false);

                theTragedyClaimedSound.Play();
                theTragedyClaimedCanvas.SetActive(true);
                yield return new WaitForSeconds(19);
                theTragedyClaimedCanvas.SetActive(false);

                WeWillContinueSound.Play();
                WeWillContinueCanvas.SetActive(true);
                yield return new WaitForSeconds(9);
                WeWillContinueCanvas.SetActive(false);
                TypingSound.Stop();

                yield return new WaitForSeconds(2);

                CreditCanvas.SetActive(true);
                whisperSound.GetComponent<AudioSource>().spatialBlend = 0;

                yield return new WaitForSeconds(10);

                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
