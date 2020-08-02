using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : AbstractPlayerController
{
    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;
    public float pushPower = 2.0f;
    public float mass = 10.0f;

    public float forceYCorrection = 0.1f;

    private Animator animator;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float curSpeed = speed * Input.GetAxis("Vertical");

        Vector3 move = forward * curSpeed;

        // Vector3 speedCorrection = pusher.lastCollisionImpulse / mass;

        // if (speedCorrection.magnitude > 0.1f && Vector3.Angle(move, speedCorrection) < 90.0f) {
        //     return;
        // }

        // move -= speedCorrection;

        if (move.magnitude > 0.1f) {
            animator.SetBool("isMove", true);
        } else {
            animator.SetBool("isMove", false);
        }

        controller.SimpleMove(move);
    }

    // this script pushes all rigidbodies that the character touches

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // return;
        Debug.Log(hit.collider);
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 yCorrection = new Vector3(0, forceYCorrection, 0);

        Debug.DrawRay(hit.point, hit.moveDirection, Color.white, 2);
        Debug.DrawRay(hit.point, hit.normal, Color.green, 2);

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        Vector3 projectedPushDir = Vector3.Project(pushDir, -hit.normal);

        Debug.DrawRay(hit.point + yCorrection, projectedPushDir, Color.blue, 2);
        Debug.DrawRay(hit.point + yCorrection, pushDir, Color.red, 2);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.


        // Apply the push
        //body.velocity = pushDir * pushPower;
        //body.AddForce(pushDir * mass * controller.velocity.magnitude, ForceMode.Impulse);
        //body.AddForce(pushDir * pushPower, ForceMode.Force);
        //body.AddForceAtPosition(pushDir * pushPower, hit.point + yCorrection, ForceMode.Force);
        body.AddForceAtPosition(projectedPushDir * pushPower, hit.point + yCorrection, ForceMode.Force);
    }
}
