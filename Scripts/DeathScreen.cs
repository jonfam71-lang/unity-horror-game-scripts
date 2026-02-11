using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel; 

    void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false); 
    }

    public void ShowDeathScreen()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true); 
    }
}
