using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UI;
using UnityEngine;

public class DoorInteractController : MonoBehaviour
{
    private bool _isAnimating = false;
    private bool _isOpen = false;
    public bool _isLocked = true;

    public List<int> _usedKeys = new List<int>();
    private readonly List<int> _correctKeys = new List<int> {2, 5, 6};

    private PlayerInteractionsController _player = null;

    [SerializeField]
    public float maxInteractDistance = 5.0f;

    [SerializeField]
    private AudioSource _doorOpenSound = null;
    private float _doorOpenSoundDelay = 0.0f;
    [SerializeField]
    private AudioSource _doorCloseSound = null;
    [SerializeField]
    private AudioSource _doorUnlockedSound = null;
    [SerializeField]
    private AudioSource _doorLockedSound = null;
    [SerializeField]
    private AudioSource _doorOpenWithCreakSound = null;


    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent<DoorTag>() && !_isAnimating)
        {
            if (_isOpen)
                CloseDoor();
            else
            {
                if (_isLocked)
                    TryOpenDoor();
                else
                    OpenDoor();
            }
        }
    }

    void CloseDoor()
    {
        _usedKeys.Clear();
        _isOpen = false;
        StartCoroutine(RotateDoor(90, 1.0f));
        _doorCloseSound.Play();
    }

    void OpenDoor()
    {
        _isOpen = true;
        StartCoroutine(RotateDoor(0, 1.0f));
        _doorOpenSound.Play();
    }

    void OpenDoorWithCreak()
    {
        _isOpen = true;
        StartCoroutine(RotateDoor(0, 4.5f));
        _doorOpenWithCreakSound.Play();
    }

    void TryOpenDoor()
    {
        
        if (_player != null && _player.GetHeldKeyNumber() != -1)
        {
            int keyNumber = _player.GetHeldKeyNumber();
            if (_usedKeys.Contains(keyNumber))
            {
                Debug.Log("You have already used this key!");
                return;
            }
            if (_usedKeys.Count < 3)
            {
                Debug.Log("Used key " + keyNumber);

                if (_usedKeys.Count < 2) // play key unlocking sound only for the first 2 keys
                    _doorUnlockedSound.Play();

                _usedKeys.Add(keyNumber);             
            }
            if (_usedKeys.Count == 3)
            {
                for(int index = 0; index < _usedKeys.Count; index++)
                {
                    if (_usedKeys[index] != _correctKeys[index])
                    {
                        _usedKeys.Clear();
                        Debug.Log("Wrong key numbers or order!");
                        _doorLockedSound.Play();
                        return;
                    }
                }                                                    
                _isLocked = false;
                OpenDoorWithCreak();
            }

        }
        else
            _doorLockedSound.Play();

    }

    IEnumerator RotateDoor(float targetAngle, float rotationDuration)
    {
        float currentAngle = transform.eulerAngles.y;
        float elapsedRotationTime = 0f;       
        _isAnimating = true;

        while (elapsedRotationTime < rotationDuration)
        {
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, elapsedRotationTime / rotationDuration);
            transform.eulerAngles = new Vector3(0, newAngle, 0);

            elapsedRotationTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
        _isAnimating = false;
    }
}
