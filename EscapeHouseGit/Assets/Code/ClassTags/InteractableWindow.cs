using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWindow : MonoBehaviour
{
    public List<int> _usedKeys = new List<int>();

    private PlayerInteractionsController _player = null;
    private string _letterTagComponent;
    public GameObject _playerPoz;

    public AudioSource playSound;

    [SerializeField]
    public float maxInteractDistance = 5.0f;
    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
        _letterTagComponent = "JumpOnWindowTag";
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent(_letterTagComponent))
        {
            _playerPoz.transform.position = new Vector3(512.591f, 28.79f, 615.982f);
            playSound.Play();
        }
    }
}
