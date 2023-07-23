using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform cameraPivot;

    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed = 7;
    public float runSpeed = 12;
    public float crouchMoveSpeed = 3;

    public float groundDrag = 10;
    public float airDrag = 2;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public bool isReadyToJump = true;

    [Header("Crouching")]
    Vector3 originalPosition;
    Vector3 crouchPosition;
    [SerializeField] float crouchAmount = 3;
    [SerializeField] float crouchSpeed;
    float crouchCurrent, crouchTarget;

    [Header("Peeking")]
    [SerializeField] float peekAngle = 50;
    [SerializeField] float peekSpeed = 50;
    Vector3 leftRot, rightRot;
    float peekCurrent, peekTarget;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode leftPeekKey = KeyCode.Q;
    public KeyCode rightPeekKey = KeyCode.E;

    [Header("Ground Check")]
    public LayerMask ground;
    public bool isGrounded;

    [HideInInspector]
    public float horizontalInput, verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState {
        walking,
        running,
        crouching,
        air,
        idle
    }

    public Text stateText;

    // Start is called before the first frame update
    void Start()
    {
        peekTarget = 0.5f;

        // Get access to rigidbody
        rb = GetComponent<Rigidbody>();

        // Set crouch
        originalPosition = cameraPivot.localPosition;
        crouchPosition = new Vector3(0, originalPosition.y - crouchAmount, 0);

        // Set peek
        leftRot = new Vector3(0, 0, peekAngle);
        rightRot = new Vector3(0, 0, -peekAngle);
    }

    void FixedUpdate() {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug
        //stateText.text = "Player state: " + state;

        // Ground check
        isGrounded = Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, 1.1f, ground);

        // Update user input
        GetUserInput();

        // Update state
        UpdateState();

        Crouch();
        Peek();

        // Handle drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    void GetUserInput() {
        // Get keyboard input (WASD)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && isReadyToJump && isGrounded) {
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    void UpdateState() {
        if (Input.GetKey(crouchKey)) {
            state = MovementState.crouching;
            moveSpeed = crouchMoveSpeed;
        } else if (isGrounded && Input.GetKey(runKey)) {
            state = MovementState.running;
            moveSpeed = runSpeed;
        } else if (isGrounded) {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        } else if (!isGrounded) {
            state = MovementState.air;
        } else {
            state = MovementState.idle;
        }
    }

    void MovePlayer() {
        // Calculate movement direction
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else 
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    void Jump() {
        // Reset y velocity. Allows to jump same distances
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump() {
        isReadyToJump = true;
    }

    void Crouch() {
        if (Input.GetKeyDown(crouchKey)) {
            crouchTarget = 1;
        } else if (Input.GetKeyUp(crouchKey)) {
            crouchTarget = 0;
        }
        crouchCurrent = Mathf.MoveTowards(crouchCurrent, crouchTarget, crouchSpeed * Time.deltaTime);
        cameraPivot.localPosition = Vector3.Lerp(originalPosition, crouchPosition, crouchCurrent);
    }

    void Peek() {
        if (Input.GetKeyDown(leftPeekKey)) {
            peekTarget = 0;
        } else if (Input.GetKeyDown(rightPeekKey)) {
            peekTarget = 1;
        } else if (Input.GetKeyUp(leftPeekKey) || Input.GetKeyUp(rightPeekKey)) {
            peekTarget = 0.5f;
        }

        peekCurrent = Mathf.MoveTowards(peekCurrent, peekTarget, peekSpeed * Time.deltaTime);
        cameraPivot.localRotation = Quaternion.Lerp(Quaternion.Euler(leftRot), Quaternion.Euler(rightRot), peekCurrent);
    }
}
