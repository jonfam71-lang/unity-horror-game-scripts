using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<int> keys = new List<int>();

    public bool HasKey(int id)
    {
        return keys.Contains(id);
    }

    public void AddKey(int id)
    {
        if (!keys.Contains(id))
            keys.Add(id);
    }
}
