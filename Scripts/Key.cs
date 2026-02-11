using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID = 1; // ID ключа
    public GameObject spawnEnemy; // враг, который появляется при взятии ключа

    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyID);           // добавляем ключ
            if (spawnEnemy != null)
                spawnEnemy.SetActive(true);   // активируем врага
            Destroy(gameObject);               // убираем ключ
        }
    }
}