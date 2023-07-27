using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody rb;

    float moveSpeed;
    public int walkSpeed, runSpeed;
    bool isRunning = false;
    public int walkToRunTransitionSpeed = 20;
    bool isTired = false;
    float breath = 35;
    public float breathCapacity = 35;
    public int breathGainSpeed, breathLoseSpeed;

    float horizontalInput, verticalInput;
    Vector3 moveDirection;

    public KeyCode runKey = KeyCode.LeftShift;

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

        // Calculate breath
        if (isRunning) {
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

        // Calculate move direction
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Move
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }
}
