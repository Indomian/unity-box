using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractPlayerController
{    
    public float turnStep = 90.0f;
    public float turnInputThreshold = 0.4f;
    public float turnDuration = 1.0f;
    public float turnThreshold = 0.01f;

    public float moveStep = 1.0f;
    public float moveDuration = 1.0f;
    public float moveThreshold = 0.01f;

    public float forceYCorrection = 0.1f;

    private Animator animator;

    private Rigidbody body;


    private bool moving = false;
    private bool turning = false;
    private int turnSign;

    private Vector3 nextPosition;
    private Vector3 movementVelocity;
    private Vector3 nextAngle;
    private float prevMovementDistance;
    private bool groundedPlayer;

    private float gravityValue = -9.81f;

    
    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<CharacterController>();
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        nextPosition = transform.position;
        nextAngle = transform.forward;
    }

    void Turn() {
        if (moving || turning) {
            return;
        }
        
        float axis = Input.GetAxis("Horizontal");
        Debug.Log(axis);
        if (Math.Abs(axis) > turnInputThreshold) {
            turning = true;
            turnSign = Math.Sign(axis);
            nextAngle = Quaternion.Euler(0, turnSign * turnStep, 0) * transform.forward;
        }
    }

    Vector3 CalculateFixedPosition(Vector3 position) {
        return new Vector3(
            (float)Math.Round(position.x / moveStep) * moveStep,
            position.y,
            (float)Math.Round(position.z / moveStep) * moveStep
        );
    }

    void Move() {
        if (turning || moving) {
            return;
        }
        
        float axis = Input.GetAxis("Vertical");

        if (Math.Abs(axis) > turnInputThreshold) {
            int moveSign = Math.Sign(axis);
            Vector3 nextMovement = transform.forward  * moveSign * moveStep;
            nextPosition = CalculateFixedPosition(transform.position + nextMovement);
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward, Color.blue, 2);
            bool canMove = true;
            if (Physics.Linecast(transform.position, nextPosition, out hit)) {
                if (hit.collider.GetComponent<Unmovable>()) {
                    canMove = false;
                } else if (hit.collider.GetComponent<Activator>()) {
                    //Check if we can move activator
                    RaycastHit activatorHit;
                    Vector3 activatorPosition = hit.collider.transform.position;
                    Vector3 activatorNextPosition = CalculateFixedPosition(activatorPosition + nextMovement);
                    Debug.DrawRay(activatorPosition, transform.forward, Color.blue, 2);
                    if (Physics.Linecast(activatorPosition, activatorNextPosition, out activatorHit)) {
                        if (!activatorHit.collider.GetComponent<ButtonController>()) {
                            canMove = false;
                        }
                    }
                }
            }

            if (!canMove) {
                Debug.DrawRay(transform.position, transform.forward, Color.white, 2);
                moving = false;
                nextPosition = transform.position;
                movementVelocity = Vector3.zero;
            } else {
                movementVelocity = nextMovement / moveDuration;
                prevMovementDistance = moveStep;
                moving = true;
            }
        }
    }

    void Update() {
        Turn();
        Move();
        
        float currentAngle = Vector3.SignedAngle(transform.forward, nextAngle, Vector3.up);

        if (turning) {
            transform.Rotate(0, turnSign * turnStep / turnDuration * Time.deltaTime, 0);    
            if (Math.Abs(currentAngle) < turnThreshold || currentAngle * turnSign < 0) {
                turning = false;
                float fixedAngle = (float)Math.Round(transform.rotation.eulerAngles.y / turnStep) * turnStep;
                float delta = -transform.rotation.eulerAngles.y + fixedAngle;
                transform.Rotate(0, delta, 0);
            }
        }

        animator.SetFloat("speed", movementVelocity.magnitude);
        animator.SetFloat("angle", currentAngle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // groundedPlayer = controller.isGrounded;
        // if (groundedPlayer && movementVelocity.y < 0)
        // {
        //     movementVelocity.y = 0f;
        // }

        // movementVelocity.y += gravityValue * Time.deltaTime;

        body.MovePosition(transform.position + movementVelocity * Time.fixedDeltaTime);

        Debug.DrawRay(transform.position, transform.forward, Color.green);
        Debug.DrawLine(transform.position, nextPosition, Color.red);
        float currentDistance = Vector3.Distance(transform.position, nextPosition);
        if (Math.Abs(currentDistance) <= moveThreshold || currentDistance > prevMovementDistance) {
            moving = false;
            transform.SetPositionAndRotation(nextPosition, transform.rotation);
            movementVelocity = Vector3.zero;
            prevMovementDistance = 0;
        } else {
            prevMovementDistance = currentDistance;
        }
    }
}
