using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsController : MonoBehaviour
{
    public Transform head;

    public float itemPickupDistance;

    Transform pickedUpItem = null;
    float pickedUpItemDistance = 0f;

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(head.position, head.forward, out hit, itemPickupDistance);

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
