using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionsController : MonoBehaviour
{
    public Transform playerHead;
    public Transform playerHand;

    public TextMeshProUGUI pickUpText;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI putItemBackText;
    public TextMeshProUGUI exitHidingSpotText;

    [SerializeField]
    private float itemPickupDistance = 5f;

    private Transform pickedItem = null;
    private Vector3 initialPosition;     // initial position of held item
    private Quaternion initialRotation;   // initial rotation of held item
    private Transform initialParent = null;          // initial parent of held item

    private Transform targetedItem = null;

    public static int globalVariableForInteractionLetters = 0;

    public void DisableTexts()
    {
        pickUpText.GetComponent<TMP_Text>().enabled = false;
        interactText.GetComponent<TMP_Text>().enabled = false;
        putItemBackText.GetComponent<TMP_Text>().enabled = false;
        exitHidingSpotText.GetComponent<TMP_Text>().enabled = false;
    }

    void Update()
    {
        RaycastHit hit;
        bool cast = Physics.Raycast(playerHead.position, playerHead.forward, out hit, itemPickupDistance);

        if (cast && hit.collider.gameObject.GetComponent<InteractableObjectTag>())  // hovering interactable object
        {
            if(targetedItem != null && targetedItem != hit.transform && targetedItem.GetComponent<Outline>() != null)   // exit hovering previous interactable object
                targetedItem.GetComponent<Outline>().enabled = false;
            targetedItem = hit.transform;      

            if (hit.transform.GetComponent<Outline>() != null) // if interactable object has outline script
            {
                if (hit.collider.gameObject.GetComponent<PickableObjectTag>() != null)
                {
                    if (pickedItem == null)
                        pickUpText.GetComponent<TMP_Text>().enabled = true;
                }                   
                else // text for any other interactable
                    interactText.GetComponent<TMP_Text>().enabled = true;

                hit.transform.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (hit.collider.gameObject.GetComponent<KeyHangerTag>() != null)
                {
                    // if interactable object is the key hanger and item in hand
                    if (pickedItem != null)
                        putItemBackText.GetComponent<TMP_Text>().enabled = true;
                }
                else // text for any other interactable
                    interactText.GetComponent<TMP_Text>().enabled = true;
            }
                

        }
        else if (targetedItem != null)  
        {
            DisableTexts();         
            if (targetedItem.GetComponent<Outline>() != null)
                targetedItem.GetComponent<Outline>().enabled = false;
        }



        if (Input.GetKeyDown(KeyCode.F))    
        {
            if (pickedItem != null) 
            {
                if (pickedItem.GetComponent<KeyTag>() != null)
                {
                    if (cast && hit.collider.gameObject.GetComponent<KeyHangerTag>() != null)
                    {
                        // put key back to hanger
                        pickedItem.SetParent(initialParent);
                        pickedItem.position = initialPosition;
                        pickedItem.rotation = initialRotation;
                        pickedItem = null;
                        DisableTexts();
                    }               
                }
                else
                {
                    // drop held item
                    pickedItem.SetParent(null);
                }
            }
            else if (cast)  // try picking item up
            {
                if (hit.collider.gameObject.GetComponent<PickableObjectTag>())
                {

                    pickedItem = hit.transform;
                    initialPosition = pickedItem.position;      // save initial state
                    initialRotation = pickedItem.rotation;
                    initialParent = hit.transform.parent;

                    GetHeldKeyNumber();

                    pickedItem.SetParent(playerHand);         // ALTERNATIVE: setParent(camera) to follow camera
                    pickedItem.position = playerHand.position;
                    pickedItem.rotation = Quaternion.Euler(playerHand.rotation.eulerAngles + new Vector3(0f, -15f, 90f));   // align to hand + offset so object is facing forward
                    DisableTexts();
                }
            }
        }
    }

    public int GetHeldKeyNumber()
    {
        if(!pickedItem)
            return -1;

        GameObject obj = pickedItem.gameObject;
        Debug.Log(!obj.GetComponent<KeyTag>());
        if (!obj.GetComponent<KeyTag>())
            return -1;
        
        string objectName = obj.name;
        int keyNumber;
        if(int.TryParse(objectName[objectName.Length - 1].ToString(), out keyNumber))
        {
            Debug.Log(keyNumber);
            return keyNumber;
        }
        else return -1;
    }

}
