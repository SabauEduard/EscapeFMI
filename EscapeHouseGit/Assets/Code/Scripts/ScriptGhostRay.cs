using System.Collections;
using UnityEngine;

public class ScriptGhostRay : MonoBehaviour
{
    private PlayerInteractionsController _player = null;

    public AudioSource leavemeAloneSound;
    public AudioSource helpSound;
    public AudioSource EitherYouSound;
    public AudioSource SeemsLikeSound;
    public GameObject flashback;
    public GameObject leavemeAlone;
    public GameObject help;
    public GameObject EitherYou;
    public GameObject SeemsLike;

    public GameObject interactTexts;
    public GameObject cubeForGhostRay;


    public GameObject CutSceneCamera;
    public GameObject mainCamera;
    public GameObject playerControl;

    public GameObject textLettter1;
    public GameObject textLettter2;

    public GameObject soundLetter1;
    public GameObject soundLetter2;

    public GameObject interactText;

    public GameObject filterFlashback;

    public AudioSource ghostCry;

    public GameObject ghostStatue;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        interactText.SetActive(false);
        _player = FindObjectOfType<PlayerInteractionsController>();
        _player.pickUpText.enabled = false;

        cubeForGhostRay.SetActive(false);

        textLettter1.SetActive(false);
        textLettter2.SetActive(false);

        soundLetter1.SetActive(false);
        soundLetter2.SetActive(false);

        filterFlashback.SetActive(true);

        StartCoroutine(PlaySubtitle());

        IEnumerator PlaySubtitle()
        {
            _player.enabled = false;
            playerControl.SetActive(false);
            mainCamera.SetActive(false);
            CutSceneCamera.SetActive(true);

            flashback.SetActive(true);
            yield return new WaitForSeconds(3);
            flashback.SetActive(false);

            yield return new WaitForSeconds(1);

            leavemeAloneSound.Play();
            leavemeAlone.SetActive(true);
            yield return new WaitForSeconds(4);
            leavemeAlone.SetActive(false);

            helpSound.Play();
            help.SetActive(true);
            yield return new WaitForSeconds(2);
            help.SetActive(false);

            EitherYouSound.Play();
            EitherYou.SetActive(true);
            yield return new WaitForSeconds(5);
            EitherYou.SetActive(false);

            CutSceneCamera.SetActive(false);

            filterFlashback.SetActive(false);

            playerControl.SetActive(true);
            _player.enabled = true;
            ghostStatue.SetActive(true);
            interactText.SetActive(true);
            mainCamera.SetActive(true);

            SeemsLikeSound.Play();
            SeemsLike.SetActive(true);
            yield return new WaitForSeconds(17);
            SeemsLike.SetActive(false);

            ghostCry.Play();
        }
    }
}
