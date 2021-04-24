using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchMovementSpeed;
    [SerializeField] float crouchSpeed;
    [Space]
    [SerializeField] float groundDistance;
    [SerializeField] float jumpHeight;
    [Space]
    [SerializeField] float headBobbingIntensity;
    [SerializeField] float walkingHeadBobFrequency;
    [SerializeField] float sprintingHeadBobFrequency;
    [SerializeField] float crouchingHeadVerticalAmplitude;
    [SerializeField] float standingHeadBobVerticalAmplitude;
    [Space]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform head;
    [SerializeField] Transform crouchCamPos;
    [SerializeField] Transform camera;
    [SerializeField] LayerMask groundMask;

    public bool crouchToggle;
    public bool sprintToggle;

    CharacterController controller;

    float gravity = -9.81f * 2;
    float yNegativeVelocity = -2;
    float crouchedHeadPos;
    float timeWalking;
    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;

    Vector3 movement;
    Vector3 velocity;

    bool isGrounded;
    bool isSprinting;
    bool isCrouched;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        crouchedHeadPos = camera.transform.position.y - 0.7f;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = yNegativeVelocity;

        //movement + sprint
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            timeWalking += Time.deltaTime;
        else
            timeWalking = 0;

        
        //jump + artificial gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        Inputs();
        Movement();
        Crouch();
        HeadBobbing();
    }
    void Inputs()
    {
        //movement
        if (isGrounded)
        {
            if (!sprintToggle)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isSprinting = true;
                    isCrouched = false;
                }
                else
                    isSprinting = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && !isSprinting)
                {
                    isSprinting = true;
                    isCrouched = false;
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting)
                    isSprinting = false;
            }

            if (!crouchToggle)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    isCrouched = true;
                    isSprinting = false;
                }
                else
                    isCrouched = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouched)
                {
                    isCrouched = true;
                    isSprinting = false;
                }
                else if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouched)
                    isCrouched = false;
            }
        }

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // jump formula: result = sqrt( h * -2 * g)
    }
    void Movement()
    {
        if (isSprinting)
            controller.Move(movement * sprintSpeed * Time.deltaTime);
        if (isCrouched)
            controller.Move(movement * crouchMovementSpeed * Time.deltaTime);
        if (!isSprinting && !isCrouched)
            controller.Move(movement * speed * Time.deltaTime);
    }
    //reparar
    void Crouch()
    {
        float height = camera.position.y;
        if (isCrouched && height > crouchCamPos.position.y + 0.5f)
            height -= Time.deltaTime * crouchSpeed;

        camera.position = new Vector3(camera.position.x, height, camera.position.z);
    }

    void HeadBobbing()
    {
        Vector3 newHeadPosition;
        if(!isCrouched)
            newHeadPosition = head.position + CalculateHeadBobOffset(timeWalking);
        else
            newHeadPosition = crouchCamPos.position + CalculateHeadBobOffset(timeWalking);

        if (isSprinting && !isCrouched)
        {
            hBobFrequency = sprintingHeadBobFrequency;
            hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
        }
        else if (!isSprinting && isCrouched)
        {
            hBobFrequency = walkingHeadBobFrequency;
            hBobVerticalAmplitude = crouchingHeadVerticalAmplitude;
        }
        else
        {
            hBobFrequency = walkingHeadBobFrequency;
            hBobVerticalAmplitude = standingHeadBobVerticalAmplitude;
        }

        if (isGrounded)
        {
            camera.position = Vector3.Lerp(camera.position, newHeadPosition, headBobbingIntensity);
            if ((camera.position - newHeadPosition).magnitude <= axisDifference)
            {
                camera.position = newHeadPosition;
            }
        }
        

    }
    Vector3 CalculateHeadBobOffset(float value)
    {
        float movement;
        Vector3 offset = Vector3.zero;
        if (value > 0)
        {
            movement = Mathf.Sin(value * hBobFrequency * 2) * hBobVerticalAmplitude;
            if (!isCrouched)
                offset = head.transform.up * movement;
            else
                offset = crouchCamPos.transform.up * movement;
        }
        return offset;
    }
}
