using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    public bool isBlocking;
    public Animator animator;


    void Update()
    {
        // ПКМ для включения/выключения блока
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isBlocking = !isBlocking;
            animator.SetBool("IsBlocking", isBlocking);
        }
    }

    public void StartBlock()
    {
        isBlocking = true;
        Debug.Log("BLOCK ON");
    }

    public void EndBlock()
    {
        isBlocking = false;
        Debug.Log("BLOCK OFF");
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnBlock(InputValue val)
    {
        bool isBlocking = val.isPressed;
        animator.SetBool("IsBlocking", isBlocking);
    }
}
