using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlaterController : MonoBehaviour
{
    [Header("Movement")]
    public float currentSpeed;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Crouch Settings")]
    public float standHeight = 2f;
    public float crouchHeight = 1f;
    public float heightLerpSpeed = 10f;
    public float crouchMoveSpeed = 2f;

    private bool _isCrouching = false;

    [Header("Jump / Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    private float _verticalVelocity;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    private bool _isGrounded;
    [SerializeField] private CharacterController _characterController;
    private Camera _playerCamera;
    private Vector2 _move;
    [SerializeField] private GameObject interactHint;


    private void Awake()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();

        _playerCamera = Camera.main;
    }

    private void Start()
    {
        currentSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        

    }


    public void OnMove(InputValue val)
    {
        _move = val.Get<Vector2>();

    }

    public void OnSprint(InputValue val)
    {
        bool sprinting = val.Get<float>() > 0.5f;

        if (!_isCrouching)
            currentSpeed = sprinting ? sprintSpeed : walkSpeed;
    }

    public void OnJump(InputValue val)
    {
        if (_isGrounded && !_isCrouching)
        {
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnCrouch(InputValue val)
    {
        _isCrouching = val.Get<float>() > 0.5f;
        currentSpeed = _isCrouching ? crouchMoveSpeed : walkSpeed;
    }

    private void Update()
    {
 
        HandleGround();
        HandleMovement();
        HandleGravity();
        HandleCrouchHeight();
        HandleSliding();
        HandleSlopeSlide();
        HandleInteraction();
        CheckDoorHint();



    }

    private void HandleSliding()
    {
        if (!_isGrounded) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height * 0.6f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            
            if (slopeAngle > _characterController.slopeLimit)
            {
                Vector3 slideDirection = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z);
                _characterController.Move(slideDirection * 5f * Time.deltaTime);
            }
        }
    }

    void CheckDoorHint()
    {
        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                interactHint.SetActive(true);
                return;
            }
        }

        interactHint.SetActive(false);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log("Попали в: " + hit.collider.name);
                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("Это дверь!");
                    PlayerInventory inv = GetComponent<PlayerInventory>();
                    Door door = hit.collider.GetComponentInParent<Door>();

                    if (door != null && inv != null)
                    {
                        door.TryOpen(inv);
                    }
                }
            }
        }
    }


    public void OnInteract(InputValue val)
    {
        Debug.Log("OnInteract вызван");

        if (!val.isPressed) return;
    }



    private void HandleSlopeSlide()
    {
        if (!_isGrounded) return;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle > _characterController.slopeLimit)
            {
                Vector3 slideDirection = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z);
                _characterController.Move(slideDirection * Time.deltaTime * 5f);
            }
        }
    }

    private void HandleGround()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;
    }

    private void HandleMovement()
    {
        Vector3 forward = Camera.main.transform.forward; forward.y = 0;
        Vector3 right = Camera.main.transform.right; right.y = 0;

        Vector3 move = forward * _move.y + right * _move.x;
        if (move.sqrMagnitude > 1) move.Normalize();

        _characterController.Move(move * currentSpeed * Time.deltaTime);

        float speedValue = new Vector3(
            _characterController.velocity.x,
            0,
            _characterController.velocity.z
        ).magnitude;

        
    }


    private void HandleGravity()
    {
        _verticalVelocity += gravity * Time.deltaTime;
        _characterController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleCrouchHeight()
    {
        float targetHeight = _isCrouching ? crouchHeight : standHeight;

       
        if (!_isCrouching)
        {
            float checkDistance = standHeight - _characterController.height;

            if (checkDistance > 0.05f)
            {
                if (Physics.SphereCast(
                    transform.position + Vector3.up * (_characterController.height * 0.5f),
                    _characterController.radius * 0.9f,
                    Vector3.up,
                    out _,
                    checkDistance))
                {
                    targetHeight = _characterController.height;
                }
            }
        }

      
        float oldHeight = _characterController.height;
        float newHeight = Mathf.Lerp(oldHeight, targetHeight, Time.deltaTime * heightLerpSpeed);

        _characterController.height = newHeight;


        if (_playerCamera != null && _playerCamera.transform.IsChildOf(transform))
        {
            float targetY = _isCrouching ? 0.5f : 1f;
            Vector3 local = _playerCamera.transform.localPosition;

            _playerCamera.transform.localPosition = new Vector3(
                local.x,
                Mathf.Lerp(local.y, targetY, Time.deltaTime * heightLerpSpeed),
                local.z
            );
        }

    }
}
