using UnityEngine;

public class PlayerDoorInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public Camera cam;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Door door = hit.collider.GetComponent<Door>();
                PlayerInventory keys = GetComponent<PlayerInventory>();

                if (door != null && keys != null)
                {
                    door.TryOpen(keys);
                }
            }
        }
    }
}
