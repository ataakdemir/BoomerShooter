using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyId;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyInventory keyInventory = other.GetComponent<KeyInventory>();


            if (keyInventory != null )
            {
                keyInventory.keys.Add(keyId);
                Debug.Log(keyId + "numaralaý anahtar eklendi");
                Destroy(gameObject);
            }
        }
    }
}
