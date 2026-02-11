using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorMesh;
    public float openAngle = 90f;
    public float speed = 3f;
    public bool isOpen = false;

    [Tooltip("0 = без ключа, >0 = нужен ключ с таким ID")]
    public int keyID = 0;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = doorMesh.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void TryOpen(PlayerInventory playerInventory)
    {
        if (keyID != 0 && !playerInventory.HasKey(keyID))
        {
            Debug.Log("Дверь заперта! Нужен ключ ID: " + keyID);
            return;
        }

        isOpen = !isOpen;
    }

    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;
        doorMesh.localRotation = Quaternion.Lerp(
            doorMesh.localRotation,
            target,
            Time.deltaTime * speed
        );
    }
}
