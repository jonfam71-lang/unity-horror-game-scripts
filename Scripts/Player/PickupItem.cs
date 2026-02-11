using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public GameObject weaponInHand; 


    bool canPickUp = false;

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player DETECTED!");
            canPickUp = true;
        }
    }

    void PickUp()
    {
        weaponInHand.SetActive(true); /
        gameObject.SetActive(false); 
    }
}

