using UnityEngine;

public class WeaponAnimationEvents : MonoBehaviour
{
    public SwordHit swordHit;

    public void EnableHit()
    {
        swordHit.EnableHit();
    }

    public void DisableHit()
    {
        swordHit.DisableHit();
    }
}