using UnityEngine;

public class PlayerCombatEvents : MonoBehaviour
{
    public SwordHit swordHit;
    public PlayerBlock playerBlock;

    public void EnableHit()
    {
        swordHit.EnableHit();
    }

    public void DisableHit()
    {
        swordHit.DisableHit();
    }

    public void StartBlock()
    {
        playerBlock.StartBlock();
    }

    public void EndBlock()
    {
        playerBlock.EndBlock();
    }
}