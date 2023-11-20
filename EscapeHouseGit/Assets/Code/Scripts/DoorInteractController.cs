using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;

public class DoorInteractController : MonoBehaviour
{
    public bool isOpen = false;
    private List<int> _usedKeys = new List<int>();
    private List<int> _correctKeys = new List<int> {2, 5, 6};

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                TryOpenDoor();
            }
        }
    }

    void CloseDoor()
    {
        _usedKeys.Clear();
        isOpen = false;
        StartCoroutine(RotateDoor(270));
    }

    void TryOpenDoor()
    {
        
        PlayerInteractionsController player = FindObjectOfType<PlayerInteractionsController>();
        if (player != null && player.GetHeldKeyNumber() != -1)
        {
            int keyNumber = player.GetHeldKeyNumber();
            if (_usedKeys.Contains(keyNumber))
            {
                Debug.Log("You have already used this key!");
                return;
            }
            if (_usedKeys.Count < 3)
            {
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
                isOpen = true;
                StartCoroutine(RotateDoor(360));
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
