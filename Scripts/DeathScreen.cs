using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; // Panel с текстом "YOU DIED"

    void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false); // Скрыть экран в начале
    }

    public void ShowDeathScreen()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true); // Показываем экран
    }
}