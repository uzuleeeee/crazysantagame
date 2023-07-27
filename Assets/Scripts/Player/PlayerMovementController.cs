using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody rb;

    public float groundDrag, airDrag;

    [Header("Horizontal movement")]
    float moveSpeed;
    public int walkSpeed, runSpeed;
    bool isRunning = false;
    public int walkToRunTransitionSpeed = 20;
    public float airMultiplier = 0.2f;

    [Header("Jump")]
    float jumpForce;
    public float normalJumpForce, latchJumpForce;
    bool isReadyToJump = true;
    public float jumpCoolDown = 0.5f;

    [Header("Latch")]
    public LayerMask latchLayer;
    public Vector3 latchTopRaycast, latchBottomRaycast;
    public float latchRaycastLength;
    public bool isLatching;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Vector3 raycastOffset = new Vector3(0, 1, 0);
    public float raycastLength = 1.1f;
    bool isGrounded;

    [Header("Breath")]
    bool isTired = false;
    float breath = 35;
    public float breathCapacity = 35;
    public int breathGainSpeed, breathLoseSpeed;

    [Header("Keys")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    float horizontalInput, verticalInput;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        breathCapacity = breath;
    }

    // Update is called once per frame
    void Update()
    {
        isRunning = Input.GetKey(runKey);
        isGrounded = Physics.Raycast(transform.position + raycastOffset, Vector3.down, raycastLength, groundLayer);
        Debug.DrawRay(transform.position + raycastOffset, Vector3.down * raycastLength, Color.red);

        bool topRay = Physics.Raycast(transform.position + latchTopRaycast, transform.forward, latchRaycastLength, latchLayer);
        bool bottomRay = Physics.Raycast(transform.position + latchBottomRaycast, transform.forward, latchRaycastLength, latchLayer);
        Debug.DrawRay(transform.position + latchTopRaycast, transform.forward * latchRaycastLength, Color.red);
        Debug.DrawRay(transform.position + latchBottomRaycast, transform.forward * latchRaycastLength, Color.red);
        isLatching = !topRay && bottomRay && rb.velocity.y < 0;
        Debug.Log(isLatching);

        // Calculate breath
        if (isRunning && !isLatching) {
            breath -= Time.deltaTime * breathLoseSpeed;
        } else {
            breath += Time.deltaTime * breathGainSpeed;
        }
        breath = Mathf.Clamp(breath, 0, breathCapacity);
        isTired = breath <= 0;

        // Calculate move speed
        if (isRunning && !isTired) {
            moveSpeed += Time.deltaTime * walkToRunTransitionSpeed;
        } else {
            moveSpeed -= Time.deltaTime * walkToRunTransitionSpeed;
        }
        moveSpeed = Mathf.Clamp(moveSpeed, walkSpeed, runSpeed);
        if (!isGrounded) {
            moveSpeed *= airMultiplier;
        } else if (isLatching) {
            moveSpeed = 0;
        }

        if (isLatching && !Input.GetKey(jumpKey)) {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        } else {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        // Calculate move direction
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Adjust drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;

        // Horizontal motion
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        // Jump
        if (((isGrounded && isReadyToJump) || isLatching) && Input.GetKey(jumpKey)) {
            if (isReadyToJump && Input.GetKey(jumpKey)) {
                if (isLatching) {
                    jumpForce = latchJumpForce;
                } else {
                    jumpForce = normalJumpForce;
                }

                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                isReadyToJump = false;
                Invoke("ResetIsReadyToJump", jumpCoolDown);
            }
        }
    }

    void ResetIsReadyToJump() {
        isReadyToJump = true;
    }
}
