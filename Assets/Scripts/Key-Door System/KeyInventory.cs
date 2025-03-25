using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    public List<int> keys = new List<int>();

    public bool hasKey(int keyId)
    {
        return keys.Contains(keyId);
    }
}
