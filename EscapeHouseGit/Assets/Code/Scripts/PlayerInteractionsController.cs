using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionsController : MonoBehaviour
{
    public Transform head;
    public TextMeshProUGUI pickUpText;

    public float itemPickupDistance;

    Transform pickedUpItem = null;
    float pickedUpItemDistance = 0f;
    Transform previousItem = null;

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(head.position, head.forward, out hit, itemPickupDistance);

        if (cast && hit.transform.CompareTag("CanBePickedUp"))
        {
            previousItem = hit.transform;

            pickUpText.GetComponent<TMP_Text>().enabled = true;

            if (hit.transform.GetComponent<Outline>() != null)
            {
                hit.transform.GetComponent<Outline>().enabled = true;
            }
        }
        else if (previousItem != null)
        {
            pickUpText.GetComponent<TMP_Text>().enabled = false;
            
            if (previousItem.GetComponent<Outline>() != null)
            {
                previousItem.GetComponent<Outline>().enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Debug.Log("F key was pressed");

            if (pickedUpItem != null)
            {
                pickedUpItem.SetParent(null);

                if (pickedUpItem.GetComponent<Rigidbody>() != null)
                {
                    pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
                }

                if (pickedUpItem.GetComponent<Collider>() != null)
                {
                    pickedUpItem.GetComponent<Collider>().enabled = true;
                }

                pickedUpItem = null;
            }
            else if (cast)
            {
                // Debug.Log("F key was pressed and hit something");

                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("CanBePickedUp"))
                {
                    // Debug.Log("F key was pressed and hit something that can be picked up");

                    pickedUpItem = hit.transform;
                    pickedUpItem.SetParent(transform);

                    pickedUpItemDistance = Vector3.Distance(head.position, pickedUpItem.position);

                    if (pickedUpItem.GetComponent<Rigidbody>() != null)
                    {
                        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
                    }

                    if (pickedUpItem.GetComponent<Collider>() != null)
                    {
                        pickedUpItem.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (pickedUpItem != null)
        {
            pickedUpItem.position = head.position + head.forward * pickedUpItemDistance;
        }
    }
}
