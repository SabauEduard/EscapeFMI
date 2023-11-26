using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UI;
using UnityEngine;

public class DoorInteractController : MonoBehaviour
{
    public bool isOpen = false;
    public bool isLocked = true;
    private List<int> _usedKeys = new List<int>();
    private List<int> _correctKeys = new List<int> {2, 5, 6};
    public float maxInteractDistance = 5.0f;
    private PlayerInteractionsController _player = null;


    private void Start()
    {
        _player = FindObjectOfType<PlayerInteractionsController>();
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(_player.playerHead.position, _player.playerHead.forward, out hit, maxInteractDistance);

        if (Input.GetKeyDown(KeyCode.F) && cast && hit.collider.gameObject.GetComponent<DoorTag>())
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                if (isLocked)
                    TryOpenDoor();
                else
                    OpenDoor();
            }
        }
    }

    void CloseDoor()
    {
        _usedKeys.Clear();
        isOpen = false;
        StartCoroutine(RotateDoor(0));
    }

    void OpenDoor()
    {
        isOpen = true;
        StartCoroutine(RotateDoor(90));
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
                        return;
                    }
                }
                Debug.Log("Door is now unlocked!");                            
                isLocked = false;
            }

        }

    }

    IEnumerator RotateDoor(float targetAngle)
    {
        float currentAngle = transform.eulerAngles.y;
        float elapsedRotationTime = 0f;
        float rotationDuration = 1f;

        while (elapsedRotationTime < rotationDuration)
        {
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, elapsedRotationTime / rotationDuration);
            transform.eulerAngles = new Vector3(0, newAngle, 0);

            elapsedRotationTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }
}
