using UnityEngine;

public class DoorTriggerPutKeyBack : MonoBehaviour
{
    PlayerInteractionsController playerInteractionsController;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteractionsController playerInteractionsController = other.GetComponent<PlayerInteractionsController>();
            if (playerInteractionsController.pickedItem != null)
                if (playerInteractionsController.pickedItem.GetComponent<KeyTag>() != null)
                {
                    // put key back to hanger
                    playerInteractionsController.pickedItem.SetParent(playerInteractionsController.initialParent);
                    playerInteractionsController.pickedItem.position = playerInteractionsController.initialPosition;
                    playerInteractionsController.pickedItem.rotation = playerInteractionsController.initialRotation;
                    playerInteractionsController.pickedItem = null;
                    playerInteractionsController.DisableTexts();
                }
        }
    }
}
