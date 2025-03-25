using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Door door; 
    private bool isPlayerNear = false; 

    private void Start()
    {
        door = GetComponentInParent<Door>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Kapýnýn önündesin, E'ye basarak açabilirsin.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E)) 
        {
            door.TryOpenDoor(); 
        }
    }
}
