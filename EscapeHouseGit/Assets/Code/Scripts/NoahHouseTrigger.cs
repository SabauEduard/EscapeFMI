using UnityEngine;

public class NoahHouseTrigger : MonoBehaviour
{
    [SerializeField]
    Transform noahDoorHouse;

    public GameObject beardMan;

    public bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        beardMan.SetActive(false);
        if (triggered == false)
        {
            triggered = true;
            if (noahDoorHouse.GetComponent<DoorInteractController>()._isOpen)
            {
                noahDoorHouse.GetComponent<DoorInteractController>().CloseDoor();
                noahDoorHouse.GetComponent<DoorInteractController>()._isLocked = true;
            }
        }
    }
}
