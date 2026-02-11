using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID = 1; 
    public GameObject spawnEnemy; 

    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyID); 
            if (spawnEnemy != null)
                spawnEnemy.SetActive(true);   
            Destroy(gameObject);              
        }
    }
}
