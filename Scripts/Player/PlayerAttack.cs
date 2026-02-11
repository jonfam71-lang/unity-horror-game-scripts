using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OnAttack(InputValue val)
    {
        if (!val.isPressed) return;

        animator.SetTrigger("Attack");
    }
}