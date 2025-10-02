using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplayer = 2.0f;
    [SerializeField] private float crouchMultiplayer = 0.5f;
    [SerializeField] private float standUpSpeed = 150f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float playerHeight = 2.0f;
    [SerializeField] private float playerCrouchHeight = 1.0f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private Transform topOfHead;

    private Vector3 currentMovement;
    private float verticalRotation;
    private float standUpVelocity = 0f;
    private float standUpOffset = 0.01f;
    RaycastHit hit;
    public bool canRotate = true;

    private float CurrentSpeed => walkSpeed 
        * (playerInputHandler.SprintTriggered ? sprintMultiplayer : 1) 
        * (playerInputHandler.CrouchTriggered ? crouchMultiplayer : 1);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleInteraction();
        HandleCrouching();
    }
    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (playerInputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void ApplyVerticalRoation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        if (!canRotate)
            return;
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRoation(mouseYRotation);
    }

    private void HandleInteraction()
    {
        if (playerInputHandler.InteractTriggered)
        {
            Debug.Log("Handle Interaction Script Triggered");
        }
    }

    private void HandleCrouching()
    {
        if (playerInputHandler.CrouchTriggered)
        {
            characterController.height = playerCrouchHeight;
        }
        else
        {
            // if wall is above, dont stand up yet
            Debug.DrawRay(topOfHead.position, Vector3.up * 0.2f, Color.red);
            if (Physics.Raycast(topOfHead.position, Vector3.up, out hit, 0.2f))
                return;

            if (characterController.height <= playerHeight - standUpOffset)
            {
                characterController.height = Mathf.SmoothDamp(characterController.height, playerHeight, ref standUpVelocity, standUpSpeed * Time.deltaTime);
            }
        }
    }
}
