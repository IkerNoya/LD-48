using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchMovementSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float slopeForce;
    [SerializeField] float slopeRayLengh;
    [SerializeField] float slideFriction;
    [SerializeField] float slideSpeed;
    [Space]
    [SerializeField] float lavaDamageValue;
    [Space]
    [SerializeField] Animator anim;
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
    [SerializeField] float slowMotionAmmount;
    [SerializeField] float slowMotionRecovery;
    [Space]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform head;
    [SerializeField] Transform crouchCamPos;
    [SerializeField] Transform camera;
    [SerializeField] LayerMask groundMask;
    [Space]
    [SerializeField] GameObject CharacterImageGood;
    [SerializeField] GameObject CharacterImageDamage;
    [SerializeField] GameObject CharacterImageDying;
    [SerializeField] GameObject Damage;
    [Space]


    public bool crouchToggle;
    public bool sprintToggle;

    CharacterController controller;
    TimeManager timeManager;
    HealthSystem health;

    float gravity = -9.81f * 2;
    float yNegativeVelocity = -2;
    float crouchedHeadPos;
    float timeWalking;
    float hBobFrequency;
    float hBobVerticalAmplitude;
    float axisDifference = 0.001f;
    float currentSlowMotionAmmount;
    float lavaTimer;
    float maxLavaTimer = 1f;
    float originalHeight;
    float originalSlideSpeed;
    float slidingFriction;
    float slideTimer = 0;
    float slideTimeLimit = 1;


    Vector3 slideForward;
    Vector3 movement;
    Vector3 velocity;

    bool isGrounded;
    bool isSprinting;
    bool isCrouched;
    bool isSlowMotionActivated = false;
    bool isInLava = false;
    bool isOnASlope = false;
    bool canSlide = true;
    bool isSliding = false;
    bool isSlidingInput = false;

    void Start()
    {
        crouchToggle = DataManager.instance.GetPlayerToggleCrouch();
        sprintToggle = DataManager.instance.GetPlayerToggleSprint();
        controller = GetComponent<CharacterController>();
        crouchedHeadPos = camera.transform.position.y - 0.7f;
        currentSlowMotionAmmount = slowMotionAmmount;
        timeManager = TimeManager.Instance;
        health = GetComponent<HealthSystem>();
        originalHeight = controller.height;
        originalSlideSpeed = slideSpeed;
    }

    void Update()
    {

        if (health.CheckDie())
        {
            SceneLoader.Get().LoadScene("GameOver");
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        Debug.DrawRay(transform.position, Vector3.down * slopeRayLengh, Color.red);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = yNegativeVelocity;
        //movement + sprint
        if (Time.timeScale != 0)
        {
            if (!isSliding)
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                float animAxis = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
                if (!OnSlope())
                {
                    movement = transform.right * x + transform.forward * z;
                    anim.SetFloat("Axis", animAxis);
                }
                anim.SetBool("isRunning", isSprinting);
            }
            anim.SetBool("isSliding", isSliding);
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                timeWalking += Time.deltaTime;
            else
                timeWalking = 0;
            //jump + artificial gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            if (!isGrounded) isSprinting = false;
            //slow motion
            if (isSlowMotionActivated) timeManager.SlowMotion(ref currentSlowMotionAmmount, isSlowMotionActivated);
            else
            {
                timeManager.SlowMotion(ref currentSlowMotionAmmount, isSlowMotionActivated);

                if (currentSlowMotionAmmount < slowMotionAmmount)
                    currentSlowMotionAmmount += Time.deltaTime * slowMotionRecovery;
            }
            if (currentSlowMotionAmmount <= 0) isSlowMotionActivated = false;

            //health + Lava
            if (isInLava)
            {
                if (lavaTimer <= 0)
                {
                    StartCoroutine(TakeDamage(0.2f));
                    health.SubstractLife(lavaDamageValue);
                    lavaTimer = maxLavaTimer;

                }
                lavaTimer -= Time.deltaTime;
            }


            if (slideTimer >= slideTimeLimit)
            {
                canSlide = true;
            }
            if (!canSlide)
                slideTimer += Time.deltaTime;


            //Functions
            Inputs();
            Movement();
            Crouch();
            HeadBobbing();
            DamageRepresentation();
        }
    }
    void Inputs()
    {
        //movement
        if (!isSliding)
            slideForward = transform.forward;
        if (!OnSlope())
        {
            if (Input.GetKey(KeyCode.C) && !isCrouched && canSlide && isGrounded)
            {
                isSliding = true;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                isSliding = false;
            }
            if (!isSliding)
            {

                if (!sprintToggle)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (isGrounded)
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
                        if (isGrounded)
                        {
                            isSprinting = true;
                            isCrouched = false;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftShift) && isSprinting)
                        isSprinting = false;
                }

                if (!crouchToggle)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (isGrounded)
                        {
                            isCrouched = true;
                            isSprinting = false;
                        }
                    }
                    else
                        isCrouched = false;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouched)
                    {
                        if (isGrounded)
                        {
                            isCrouched = true;
                            isSprinting = false;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouched)
                        isCrouched = false;
                }
            }
        }

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // jump formula: result = sqrt( h * -2 * g)
            isSprinting = false;
        }

        //slow motion
        if (Input.GetKeyDown(KeyCode.F) && currentSlowMotionAmmount > slowMotionAmmount / 4 && !isSlowMotionActivated)
        {
            isSlowMotionActivated = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isSlowMotionActivated)
        {
            isSlowMotionActivated = false;
        }
    }
    void Movement()
    {

        if (isSprinting)
        {
            slidingFriction = 10;
            if (!isInLava)
                controller.Move(movement * sprintSpeed * Time.deltaTime);
            else
                controller.Move(movement * (sprintSpeed / 2) * Time.deltaTime);
        }
        if (isCrouched && !isSliding)
        {
            if (!isInLava)
                controller.Move(movement * crouchMovementSpeed * Time.deltaTime);
            else
                controller.Move(movement * (crouchMovementSpeed / 2) * Time.deltaTime);

        }
        if (!isSprinting && !isCrouched)
        {
            slidingFriction = 7;
            if (!isInLava)
                controller.Move(movement * speed * Time.deltaTime);
            else
                controller.Move(movement * (speed / 2) * Time.deltaTime);
        }

        if (OnSlope())
        {
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }

        if (isSliding)
        {
            Slide();
        }
        else
        {
            slideSpeed = originalSlideSpeed;
        }
    }
    //reparar
    void Crouch()
    {
        float height = camera.position.y;
        if (isCrouched && height > crouchCamPos.position.y + 0.5f)
            height -= Time.deltaTime * crouchSpeed;

        camera.position = new Vector3(camera.position.x, height, camera.position.z);
    }

    bool OnSlope()
    {
        if (!isGrounded) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeRayLengh))
        {
            if (hit.normal.y < 0.98f)
            {
                Vector3 normal = hit.normal;
                Vector3 groundParalell = Vector3.Cross(transform.up, normal);
                Vector3 slopeParalell = Vector3.Cross(groundParalell, normal);
                Debug.DrawRay(hit.point, slopeParalell * 10, Color.green);
                float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, transform.up));
                if (currentSlope >= controller.slopeLimit)
                {
                    movement += slopeParalell.normalized / 2 * Time.deltaTime;
                    return true;
                }
                else return false;
            }
        }

        return false;
    }

    void Slide()
    {
        isSlidingInput = true;
        isCrouched = true;
        controller.Move(slideForward * slideSpeed * Time.deltaTime);
        slideSpeed -= Time.deltaTime * slidingFriction;
        if (slideSpeed <= 0)
        {
            controller.Move(Vector3.zero);
            slideTimer = 0;
            canSlide = false;
            GetUp();
        }
    }

    void GetUp()
    {
        isSlidingInput = false;
        isCrouched = false;
        slideSpeed = originalSlideSpeed;
    }

    void HeadBobbing()
    {
        Vector3 newHeadPosition;
        if (!isCrouched)
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

    void DamageRepresentation()
    {
        if(health.GetLife() > health.GetMaxLife() - health.GetMaxLife() / 3)
        {
            CharacterImageGood.SetActive(true);
            CharacterImageDamage.SetActive(false);
            CharacterImageDying.SetActive(false);
        }
        else if (health.GetLife() < health.GetMaxLife() - (health.GetMaxLife() / 3) && health.GetLife() > health.GetMaxLife() - (health.GetMaxLife() / 3 * 2))
        {
            CharacterImageGood.SetActive(false);
            CharacterImageDamage.SetActive(true);
            CharacterImageDying.SetActive(false);
        }
        else if (health.GetLife() < health.GetMaxLife() - (health.GetMaxLife() / 3 * 2))
        {
            CharacterImageGood.SetActive(false);
            CharacterImageDamage.SetActive(false);
            CharacterImageDying.SetActive(true);
        }

    }

    public IEnumerator TakeDamage(float timer)
    {
        Damage.SetActive(true);
        yield return new WaitForSeconds(timer);
        Damage.SetActive(false);
        yield return null;
    }
    public float GetSlowMotionAmmount()
    {
        return currentSlowMotionAmmount;
    }
    public bool GetIsInLava()
    {
        return isInLava;
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Lava"))
        {
            if (!isInLava)
                isInLava = true;
        }
        if (hit.collider.CompareTag("Ground"))
        {
            if (isInLava)
                isInLava = false;
        }
    }
}
