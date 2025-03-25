using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int requiredKeyId;
    private bool isOpen = false;

    public void TryOpenDoor()
    {
        if (isOpen) return;

        KeyInventory keyInventory = GameObject.FindWithTag("Player").GetComponent<KeyInventory>();

        if (keyInventory != null && keyInventory.hasKey(requiredKeyId)) 
        {
            OpenDoor(); 
        }
        else
        {
            Debug.Log(requiredKeyId + " numaralý anahtarý bulmalýsýn!");
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        transform.DOMove(transform.position + new Vector3(0, 10, 0), 2f).SetEase(Ease.InOutSine);
    }
}
