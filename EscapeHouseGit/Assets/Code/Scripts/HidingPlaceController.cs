using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HidingPlaceController : MonoBehaviour
{
    public float maxInteractDistance;
    public GameObject player, hidingPlayer;
    public Transform enemyTransform;
    public int hidingPlaceTagNumber;

    private bool _isHiding;
    private PlayerInteractionsController _playerController;
    private EnemyController _enemyController;
    private string _hidingPlaceTag;

    private int layerMask = ~(1 << 1);

    private void Start()
    {
        _isHiding = false;
        _playerController = FindObjectOfType<PlayerInteractionsController>();
        _enemyController = FindObjectOfType<EnemyController>();
        _hidingPlaceTag = "HidingPlaceTag" + hidingPlaceTagNumber;
    }

    private void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_playerController.playerHead.position, _playerController.playerHead.forward, out hit, maxInteractDistance, layerMask);     

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_hidingPlaceTag))
        {                  
            hidingPlayer.SetActive(true);
            hidingPlayer.GetComponentInChildren<Camera>().enabled = true;
            hidingPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = true; 
            _playerController.DisableTexts();
            _playerController.exitHidingSpotText.GetComponent<TMP_Text>().enabled = true;

            float distance = Vector3.Distance(enemyTransform.position, player.transform.position);
            if (!_enemyController.shouldCatchIfGoingToHiding() && _enemyController.chasing)
            {
                StartCoroutine(DelayedStopChase(distance));
            }

            _isHiding = true;
            player.SetActive(false);
        }
        if(_isHiding == true && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space)))
        {
            player.SetActive(true);
            hidingPlayer.GetComponentInChildren<Camera>().enabled = false;
            hidingPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled = false;
            hidingPlayer.SetActive(false);
            _isHiding = false;
            _playerController.DisableTexts();
        }
    }

    IEnumerator DelayedStopChase(float distance)
    {
        float scaledDelay = Mathf.Clamp(distance * 0.1f, 0f, 8.0f);
        yield return new WaitForSeconds(scaledDelay);
        _enemyController.stopChase();
    }
}
