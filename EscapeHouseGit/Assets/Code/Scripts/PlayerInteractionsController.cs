using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionsController : MonoBehaviour
{
    public Transform playerHead;
    public Transform playerHand;
    public TextMeshProUGUI pickUpText;

    [SerializeField] private float itemPickupDistance = 5f;

    private Transform pickedItem = null;
    private Transform targetedItem = null;

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(playerHead.position, playerHead.forward, out hit, itemPickupDistance);

        if (cast && hit.transform.CompareTag("CanBePickedUp"))  // hovering interactable object
        {
            targetedItem = hit.transform;

            pickUpText.GetComponent<TMP_Text>().enabled = true;

            if (hit.transform.GetComponent<Outline>() != null && pickedItem == null)
                hit.transform.GetComponent<Outline>().enabled = true;
        }
        else if (targetedItem != null)  // exit hovering interactable object
        {
            pickUpText.GetComponent<TMP_Text>().enabled = false;
            
            if (targetedItem.GetComponent<Outline>() != null)
                targetedItem.GetComponent<Outline>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.F))    
        {
            if (pickedItem != null)     // drop held item
            {
                pickedItem.SetParent(null);

                if (pickedItem.GetComponent<Rigidbody>() != null)
                    pickedItem.GetComponent<Rigidbody>().isKinematic = false;

                if (pickedItem.GetComponent<Collider>() != null)
                    pickedItem.GetComponent<Collider>().enabled = true;

                pickedItem = null;
            }
            else if (cast)  // try picking item up
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("CanBePickedUp"))
                {
                    pickedItem = hit.transform;

                    pickedItem.SetParent(playerHand);         // setParent(camera) to follow camera
                    pickedItem.position = playerHand.position;
                    pickedItem.rotation = Quaternion.Euler(playerHand.rotation.eulerAngles + new Vector3(0f, -15f, 90f));   // align to hand + offset so object is facing forward

                    if (pickedItem.GetComponent<Rigidbody>() != null)
                        pickedItem.GetComponent<Rigidbody>().isKinematic = true;

                    if (pickedItem.GetComponent<Collider>() != null)
                        pickedItem.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

}
