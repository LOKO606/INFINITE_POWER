using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControllerFloaty : MonoBehaviour
{
    [Header("Speed particle")]
    public ParticleSystem speedParticle;
    public float minimumSpeed;
    public float maximumSpeed;

    [Header("Movement")]
    [Tooltip("Transform that stores the direction for X and Z movement")]
    [SerializeField] private Transform orientationTransform;
    public float acceleration = 150f;
    public float maxSpeed = 7;
    public float jumpForce = 500;

    [Header("Spring")]
    public float springStrenght = 700;
    public float springDamper = 30;
    public float springRayLenght = 2;
    public float springTargetHeight = 1.7f;
    [Tooltip("For consecutives jumps, some forces can affect the jump heigh, so making this one a bit bigger than rideHeigh may be necessary")]
    public float groundedRayDistance = 1.75f;

    [Header("Ground")]
    [Tooltip("Transform that stores the direction of slopes")]
    [SerializeField] private Transform groundNormalDirectionTransform;
    [SerializeField] private LayerMask groundLayer;

    [Header("Slide")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float slideColliderHeigh;
    [SerializeField] private float slideColliderYCenter;
    [SerializeField] private float slideCameraYPosition;
    [SerializeField] private float slideMaxSpeed;
    [SerializeField] private ParticleSystem slideParticles;
    [SerializeField] private AudioSource slideSound;

    [Header("Dash:")]
    [SerializeField] private AudioSource dashSound;
    public float dashDelay = 1f;
    public float dashDuration = 0.1f;
    public float dashStrength = 50f;

    [Header("Debugs:")]
    public bool grounded;
    public bool isSliding;
    public bool disabledSpring;
    public bool jumpBuffer;
    public bool pressedDash;
    public bool isDashing;
    public bool isWall;
    public bool canDash = true;
    public bool pressedSlide = false;

    [HideInInspector] public int dashesDone = 0;
    private float defaultColliderHeigh, defaultColliderYCenter, defaultCameraYPosition, defaultMaxSpeed;
    private float horizontalInput, verticalInput;
    private bool coyoteTime, canJump = true, hasDashCharge = true;
    private bool pressedSlideContact;
    private RaycastHit hit;
    private Rigidbody playerRigidBody;
    private CapsuleCollider capsuleCollider;
    private Vector3 inputDirectionMoveGroundNormal, cameraDirectionMove, wallNormalDirection, localVel;
    private HudManager hudManager;
    private CameraShake camShake;
    void Start()
    {
        GetComponents();
        SetDefaultValues();
    }
    void GetComponents()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        hudManager = FindObjectOfType<HudManager>();
        camShake = FindObjectOfType<CameraShake>();
    }
    void SetDefaultValues()
    {
        defaultColliderHeigh = capsuleCollider.height;
        defaultColliderYCenter = capsuleCollider.center.y;
        defaultCameraYPosition = playerCamera.localPosition.y;
        defaultMaxSpeed = maxSpeed;
        wallNormalDirection = Vector3.up;
    }

    void Update()
    {
        GetInputs();
    }
    void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse1) && !pressedDash && !isSliding)
        {
            pressedDash = true;
            Invoke(nameof(StopDashing), dashDuration);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !pressedDash)
        {
            jumpBuffer = true;
            CancelInvoke("DiscardJump");
            Invoke(nameof(DiscardJumpBuffer), 0.25f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && (horizontalInput != 0 || verticalInput != 0))
        {
            pressedSlide = true;
            pressedSlideContact = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || jumpBuffer)
        {
            pressedSlide = false;
            pressedSlideContact = false;
        }
    }
    void StopDashing()
    {
        pressedDash = false;
        if (isDashing)
        {
            isDashing = false;
            if (!isSliding)
            {
                playerRigidBody.velocity = playerRigidBody.velocity * 0.5f;
            }
        }
    }
    void DiscardJumpBuffer()
    {
        jumpBuffer = false;
    }

    void FixedUpdate()
    {
        DoSpeedParticles();

        switch (GetSlideState())
        {
            case 1:
                SetSlideState(true);
                break;
            case 2:
                SetSlideState(false);
                break;
        }

        if (ValidJump())
        {
            Jump();
        }

        Spring();

        CheckGround();

        if (((dashesDone < 1 && canDash) || (canDash && grounded)))
        {
            hasDashCharge = true;
            hudManager.dashEnabled = true;
        }
        else
        {
            hasDashCharge = false;
            hudManager.dashEnabled = false;
        }

        if (!pressedDash)
        {
            CalculateMovement();

            if (grounded)
            {
                GroundMovement();
            }
            else
            {
                AirMovement();
            }
        }
        else if (pressedDash && hasDashCharge)
        {
            if (!isDashing)
            {
                camShake.ShakeCamera(1.2f, 1.2f, 1.2f, 0.2f, 20, 20, 5);
                dashSound.Play();
                dashesDone++;
                canDash = false;
                hudManager.dashEnabled = false;
                Invoke(nameof(enableDash), dashDelay);
                isDashing = true;
            }

            cameraDirectionMove = playerCamera.transform.forward;

            playerRigidBody.velocity = cameraDirectionMove.normalized * dashStrength;
        }
    }

    public void enableDash()
    {
        canDash = true;
    }

    void DoSpeedParticles()
    {
        var emission = speedParticle.emission;
        Vector3 localVel2 = orientationTransform.transform.InverseTransformDirection(playerRigidBody.velocity);
        localVel2.y = 0;
        if (localVel2.z > maximumSpeed)
        {
            emission.rateOverTime = 120;
        }
        else if (localVel2.z > minimumSpeed)
        {
            emission.rateOverTime = 40;
        }
        else
        {
            emission.rateOverTime = 0;
        }
    }

    int GetSlideState()
    {
        Vector3 velocityHolder = playerRigidBody.velocity;
        velocityHolder.y = 0;
        bool canStartSlide = !isSliding && pressedSlide && grounded && velocityHolder.magnitude >= 7 && (horizontalInput != 0 || verticalInput != 0);
        bool canStopSlide = isSliding && !pressedSlide && !Physics.Raycast(playerRigidBody.transform.position - Vector3.up, Vector3.up, 1, groundLayer);
        bool forceStopSlide = (velocityHolder.magnitude < 7 && isSliding) || (!pressedSlide && isSliding && canStopSlide);
        if (canStartSlide)
        {
            return 1;
        }
        else if (canStopSlide)
        {
            return 2;
        }
        else if (forceStopSlide)
        {
            return 2;
        }
        return 0;
    }
    public void SetSlideState(bool state)
    {
        if (state)
        {
            isSliding = true;

            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, slideCameraYPosition, playerCamera.localPosition.z);
            capsuleCollider.center = new Vector3(0, slideColliderYCenter, 0);
            capsuleCollider.height = slideColliderHeigh;

            //maintains the momentum
            if (new Vector3(playerRigidBody.velocity.x, 0, playerRigidBody.velocity.z).magnitude > slideMaxSpeed)
            {
                maxSpeed = playerRigidBody.velocity.magnitude;
            }
            else
            {
                maxSpeed = slideMaxSpeed;
            }

            pressedSlideContact = false;

            slideParticles.transform.forward = inputDirectionMoveGroundNormal;
            slideParticles.Play();
            slideSound.Play();
        }
        else
        {
            isSliding = false;
            pressedSlide = false;

            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, defaultCameraYPosition, playerCamera.localPosition.z);
            capsuleCollider.center = new Vector3(0, defaultColliderYCenter, 0);
            capsuleCollider.height = defaultColliderHeigh;
            maxSpeed = defaultMaxSpeed;

            slideParticles.Clear();
            slideParticles.Stop();
            slideSound.Stop();
        }
    }

    void Spring()
    {
        if (Physics.Raycast(playerRigidBody.transform.position, Vector3.down, out hit, springRayLenght, groundLayer) && !disabledSpring)
        {
            Vector3 vel = playerRigidBody.velocity;
            Vector3 rayDir = transform.TransformDirection(Vector3.down);
            Vector3 otherVel = Vector3.zero;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float x = hit.distance - springTargetHeight;

            float springForce = (x * springStrenght) - (relVel * springDamper);

            playerRigidBody.AddForce(rayDir * springForce);
        }
        else
        {
            hit.normal = Vector3.up;
        }

        Debug.DrawRay(playerRigidBody.transform.position, Vector3.down * springRayLenght, Color.red);
    }

    void CheckGround()
    {
        if (RayDistanceValidForGround() || RayDistanceValidForGroundGoingDownSlopes())
        {
            grounded = true;
            dashesDone = 0;
        }
        else if (grounded)
        {
            grounded = false;
            coyoteTime = true;
            Invoke(nameof(DisableCoyoteTime), 0.15f);
        }
    }
    void DisableCoyoteTime()
    {
        coyoteTime = false;
    }
    bool RayDistanceValidForGround()
    {
        if (hit.distance < groundedRayDistance && hit.distance > 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool RayDistanceValidForGroundGoingDownSlopes()
    {
        float minimumSlopeAngle = 16;
        float increasedGroundRayDistance = groundedRayDistance * 1.1f;
        bool goingDownOnSlope = Vector3.Angle(hit.normal, Vector3.up) > minimumSlopeAngle && playerRigidBody.velocity.y < 0;
        if (goingDownOnSlope && hit.distance < increasedGroundRayDistance && hit.distance > 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CalculateMovement()
    {
        SetGroundNormalDirectionTransform();

        if (!isSliding)
        {
            SetDirectionMoveWithInputs();
        }

        Debug.DrawRay(playerRigidBody.transform.position, inputDirectionMoveGroundNormal * springRayLenght, Color.red);
    }
    void SetGroundNormalDirectionTransform()
    {
        Vector3 groundNormalFoward = Vector3.Cross(orientationTransform.transform.right, hit.normal);
        groundNormalDirectionTransform.transform.forward = groundNormalFoward;
    }
    void SetDirectionMoveWithInputs()
    {
        inputDirectionMoveGroundNormal = groundNormalDirectionTransform.transform.forward * verticalInput + groundNormalDirectionTransform.transform.right * horizontalInput;
        inputDirectionMoveGroundNormal.Normalize();
    }

    void GroundMovement()
    {
        playerRigidBody.AddForce(inputDirectionMoveGroundNormal * acceleration);
        if (!pressedSlideContact)
        {
            if (!disabledSpring)
            {
                LimitVelocity();
            }
            StopMovement();
        }
    }
    void LimitVelocity()
    {
        Vector3 velocityHolder = playerRigidBody.velocity;
        velocityHolder.y = 0;

        if (velocityHolder.magnitude > maxSpeed)
        {
            velocityHolder = velocityHolder.normalized * maxSpeed;
        }

        velocityHolder.y = playerRigidBody.velocity.y;
        playerRigidBody.velocity = velocityHolder;
    }
    void StopMovement()
    {
        playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x * 0.899f, playerRigidBody.velocity.y, playerRigidBody.velocity.z * 0.899f);
    }

    void AirMovement()
    {
        localVel = orientationTransform.transform.InverseTransformDirection(playerRigidBody.velocity);
        localVel.y = 0;
        Vector3 targetVel = new Vector3(horizontalInput, 0, verticalInput).normalized;
        float targetVelMultiplier = 1.25f;
        targetVel *= (maxSpeed * targetVelMultiplier); //needs to be increased because in ground the maxSpeeds gets surpassed
        if (targetVel != Vector3.zero)
        {
            if (CanApplyforcesInThisAxis(targetVel.z, localVel.z))
            {
                playerRigidBody.AddForce(AccelerationToApplyInThisAxis(targetVel.z, localVel.z) * orientationTransform.forward);
            }
            if (CanApplyforcesInThisAxis(targetVel.x, localVel.x))
            {
                playerRigidBody.AddForce(AccelerationToApplyInThisAxis(targetVel.x, localVel.x) * orientationTransform.right);
            }
        }
    }
    bool CanApplyforcesInThisAxis(float targetVelAxis, float localVelAxis)
    {
        bool haveInput = targetVelAxis != 0;
        bool isMoving = localVelAxis > 1 || localVelAxis < -1;
        return ((haveInput || isMoving) && !isSliding);
    }
    float AccelerationToApplyInThisAxis(float targetVelAxis, float localVelAxis)
    {
        float airAcceleration = acceleration * 0.15f;
        float maxVelocityToInfluence = 11;

        bool diagonalInput = (horizontalInput != 0 && verticalInput != 0);
        bool diagonalVelocityHigherThanNormalized = (Mathf.Abs(localVel.x) > 8 && Mathf.Abs(localVel.z) > 8);

        if (diagonalInput && diagonalVelocityHigherThanNormalized)
        {
            maxVelocityToInfluence = 8;
        }

        bool targetOppositeToLocal = (localVelAxis * targetVelAxis) < 0;

        if ((localVelAxis < targetVelAxis && localVelAxis > -maxVelocityToInfluence) || (targetOppositeToLocal && targetVelAxis > 0))
        {
            return airAcceleration;
        }
        else if ((localVelAxis > targetVelAxis && localVelAxis < maxVelocityToInfluence) || (targetOppositeToLocal && targetVelAxis < 0))
        {
            return -airAcceleration;
        }

        return 0;
    }

    bool ValidJump()
    {
        if ((grounded || coyoteTime || isWall) && jumpBuffer && canJump && !isSliding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Jump()
    {
        RemoveVerticalVelocityInfluenceOnJump();

        if (isWall)
        {
            playerRigidBody.AddForce(Vector3.up * (jumpForce * 1.5f)); //compensates the missing hit.normal with 50 percent increase on up
            playerRigidBody.AddForce(wallNormalDirection * jumpForce / 2);
        }
        else
        {
            playerRigidBody.AddForce(Vector3.up * jumpForce);
            playerRigidBody.AddForce(hit.normal * jumpForce / 2);
        }


        jumpBuffer = false;
        coyoteTime = false;

        disabledSpring = true;
        Invoke(nameof(EnableSpring), 0.3f);
        canJump = false;
        Invoke(nameof(CanJumpAgain), 0.25f);
    }
    void RemoveVerticalVelocityInfluenceOnJump()
    {
        playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, 0, playerRigidBody.velocity.z);
    }

    void CanJumpAgain()
    {
        canJump = true;
    }

    void EnableSpring()
    {
        disabledSpring = false;
    }

    public void DisableSpringAfterExplosion()
    {
        disabledSpring = true;
        Invoke(nameof(EnableSpring), 0.15f);
    }
    void OnCollisionStay(Collision collisionInfo)
    {

        if (!grounded && collisionInfo.transform.tag != "NoClimb")
        {
            wallNormalDirection = collisionInfo.contacts[0].normal;
            float angle = Vector3.Angle(Vector3.up, collisionInfo.contacts[0].normal);
            bool validAngleForWall = (angle <= 95 && angle >= 80);
            if (validAngleForWall)
            {
                isWall = true;
                if (playerRigidBody.velocity.y < -0 && (horizontalInput != 0 || verticalInput != 0))
                {
                    playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, 0, playerRigidBody.velocity.z);
                }

            }
        }
        else
        {
            isWall = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.transform.tag != "NoClimb")
        {
            coyoteTime = true;
            CancelInvoke(nameof(DisableCoyoteTime));
            Invoke(nameof(DisableCoyoteTime), 0.15f);
        }
        isWall = false;
        wallNormalDirection = Vector3.up;
    }
}