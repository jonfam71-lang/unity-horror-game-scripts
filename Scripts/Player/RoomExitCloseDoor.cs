using UnityEngine;

public class RoomExitCloseDoor : MonoBehaviour
{
    public  Door door; // ссылка на дверь

    private void OnTriggerExit(Collider other)
    {
        // проверяем, что вышел именно игрок
        if (other.CompareTag("Player"))
        {
            // если дверь открыта — закрываем
            if (door.isOpen)
            {
                door.isOpen = false;
                Debug.Log("Игрок вышел из комнаты — дверь закрыта");
            }
        }
    }
}