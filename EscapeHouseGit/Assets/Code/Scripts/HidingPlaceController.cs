using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlaceController : MonoBehaviour
{
    public float loseDistance, maxInteractDistance;
    public GameObject player, hidingPlayer;
    public Transform enemyTransform;

    private bool _isHiding;
    private PlayerInteractionsController _playerController;
    private EnemyController _enemyController;

    private void Start()
    {
        _isHiding = false;
        _playerController = FindObjectOfType<PlayerInteractionsController>();
        _enemyController = FindObjectOfType<EnemyController>();
    }

    private void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_playerController.playerHead.position, _playerController.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent<HidingPlaceTag>())
        {
            hidingPlayer.SetActive(true);
            float distance = Vector3.Distance(enemyTransform.position, player.transform.position);
            if (distance > loseDistance && _enemyController.chasing)
            {
                _enemyController.stopChase();
            }
            _isHiding = true;
            player.SetActive(false);
        }
        if(_isHiding == true && Input.GetKeyDown(KeyCode.Q))
        {
            player.SetActive(true);
            hidingPlayer.SetActive(false);
            _isHiding = false;
        }
    }
}
