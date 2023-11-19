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
    private Vector3 initialPosition;     // initial position of held item
    private Quaternion initialRotation;   // initial rotation of held item
    private Transform initialParent = null;          // initial parent of held item
    
    private Transform targetedItem = null;

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(playerHead.position, playerHead.forward, out hit, itemPickupDistance);

        if (cast && hit.collider.gameObject.GetComponent<InteractableObjectTag>())  // hovering interactable object
        {
            if(targetedItem != null && targetedItem != hit.transform && targetedItem.GetComponent<Outline>() != null)   // exit hovering previous interactable object
                targetedItem.GetComponent<Outline>().enabled = false;
            targetedItem = hit.transform;

            

            if (hit.transform.GetComponent<Outline>() != null && pickedItem == null)
            {
                pickUpText.GetComponent<TMP_Text>().enabled = true;
                hit.transform.GetComponent<Outline>().enabled = true;
            }
                
        }
        else if (targetedItem != null)  // if stopped hovering interactable object
        {
            pickUpText.GetComponent<TMP_Text>().enabled = false;
            
            if (targetedItem.GetComponent<Outline>() != null)
                targetedItem.GetComponent<Outline>().enabled = false;
        }



        if (Input.GetKeyDown(KeyCode.F))    
        {
            if (pickedItem != null)     // drop held item
            {

                Debug.Log(pickedItem.position);
                Debug.Log(initialPosition);  
                pickedItem.SetParent(initialParent);
                Debug.Log(pickedItem.position);
                pickedItem.position = initialPosition;
                Debug.Log(pickedItem.position);
                pickedItem.rotation = initialRotation;

              //  if (pickedItem.GetComponent<Rigidbody>() != null)
                //    pickedItem.GetComponent<Rigidbody>().isKinematic = false;

             //   if (pickedItem.GetComponent<Collider>() != null
              //      && pickedItem.GetComponent<Collider>().enabled == false)
              //      pickedItem.GetComponent<Collider>().enabled = true;

                pickedItem = null;
            }
            else if (cast)  // try picking item up
            {

                if (hit.collider.gameObject.GetComponent<PickableObjectTag>())
                {
                    pickedItem = hit.transform;
                    initialPosition = pickedItem.position;
                    initialRotation = pickedItem.rotation;
                    initialParent = hit.transform.parent;

                    pickedItem.SetParent(playerHand);         // setParent(camera) to follow camera
                    pickedItem.position = playerHand.position;
                    pickedItem.rotation = Quaternion.Euler(playerHand.rotation.eulerAngles + new Vector3(0f, -15f, 90f));   // align to hand + offset so object is facing forward

                   // if (pickedItem.GetComponent<Rigidbody>() != null)
                     //   pickedItem.GetComponent<Rigidbody>().isKinematic = true;

                  //  if (pickedItem.GetComponent<Collider>() != null)
                     //   pickedItem.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }

}
