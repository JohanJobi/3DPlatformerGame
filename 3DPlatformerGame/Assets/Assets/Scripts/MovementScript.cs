
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class MovementScript : MonoBehaviour
{
    [Header("Refrences")]
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    public Transform groundCheck;
    public Rigidbody rb;
    public BoxCollider Boxcollider;


    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool existingSlope;


    [Header("Checks")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Input")]
    bool isGrounded;
    bool isJumping;
    

    [Header("Jump")]
    public float jumpButtonGracePeriod;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    public float jumpHeight = 3f;


    [Header("Physics")]
    public float maxSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.8f;
    float turnSmoothVelocity;
    Vector3 velocity;

    [Header("Grappling")]
    Vector3 moveDirection;
    public bool activeGrapple;
    public bool freeze;
    private Vector3 VelocityToSet;
    private bool enableMovemenentOnNextTouch;



    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Boxcollider = GetComponent<BoxCollider>();

        controller.enabled = true;  // box collider and rigidbody disabled for reguluar movement
        Boxcollider.enabled = false;
        rb.isKinematic = true;
    }

    void Update()
    {
        Movement(); // for future development of the code I have used the void update to call the Movement Function

    }

    private void Movement()
    {
        if (activeGrapple) return;
        // if active grapple then the player doesnt move
        //if (freeze) velocity = Vector3.zero; 
        // make the velocity 0 for grappling so you dont continue to move (you freeze)
        if (freeze)
        {
            rb.velocity = Vector3.zero;
            maxSpeed = 0f;
            
        }
        else
        {
            maxSpeed = 6f;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // implemets gravity  when grounded
        }
        float horizontal = Input.GetAxisRaw("Horizontal"); //gets horizontal and vertical input from mouse
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;// direction which you are looking, .normalized
                                                                             // takes the vector of the same direction and makes the length 1
        float inputMagnitude = Mathf.Clamp01(direction.magnitude); //clamps magnitude between 0 and 1 for value in animator blend
                                                                   //tree
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputMagnitude *= 2; //increases speed of animation when running
        }
        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
        float speed = inputMagnitude * maxSpeed;
        // this is useful as without it, moving in a diagonal will make it faster

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isMoving", true);//for animation

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            // finds target rotation for the character and adds current y value of camera to determine where the character should face
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 
            //smooths the rotation angle towards target angle to make it more smoother
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // moves direction of character when you start moving and change the mouse direction
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            //moves the character at a consistent speed regardless of the input direction magnitude the time.deltatime
            //makes for smooth movement independant of the fram rate
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (isGrounded)
        {
            lastGroundedTime = Time.time; //find time of last grounded time
        }
        if (Input.GetButton("Jump"))
        {
            jumpButtonPressedTime = Time.time; //find the time since jump pressed
        }

        if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {// this is used to make the jump more responsive and uses a grace period so that you dont have to press the jump
         // exactly at the frame where the player is grounded making parkour a bit easier for the player
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
            isJumping = true;
            jumpButtonPressedTime = null;
            lastGroundedTime = null;

        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {//animations
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                animator.SetBool("isJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            if ((isJumping && velocity.y < 0) || velocity.y < -2)
            {
                animator.SetBool("isFalling", true);
            }
        }
    }




    private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt (2* (displacementY - trajectoryHeight) / gravity));  
                                                                             
        return velocityXZ + velocityY; // calculates jump velocity returns the y and x velocities
    }



    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {


        activeGrapple = true;
        VelocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        // delay in time so that it doesnt apply when the movement control function is stil
        //  active

    }
 


    private void SetVelocity()
    {
        controller.enabled = false;
        Boxcollider.enabled = true;
        rb.isKinematic = false;   
        // box collider and rigidbody enabled for giving a force to the character

        enableMovemenentOnNextTouch = true;
        rb.velocity = VelocityToSet;

       // to push the player into the direction of grapple

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovemenentOnNextTouch)
        {
            enableMovemenentOnNextTouch = false;
            activeGrapple = false;
            controller.enabled = true;
            Boxcollider.enabled = false;  // box collider and rigidbody enabled for grappling
            rb.isKinematic = true;
            GetComponent<Grappling>().StopGrapple();
        }
    }



}



