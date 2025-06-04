using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;

    private float x, z;
    private float runSpeed = 20f;

    private float gravity = -95f;

    Vector3 velocity;

    public Transform groundCheck;
    private float sphereRadius = 1f;
    public LayerMask groundMask;

    bool isGrounded;

    private float jumpHeight = 3f;

    public bool isSprinting;

    public Animator animator;

    void Update()
    {
        movimiento();
        salto();
        velocity.y += gravity * Time.deltaTime;
    }

    private void movimiento()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelZ", z);
        animator.SetBool("IsSprinting", Input.GetKey(KeyCode.LeftShift));

        Vector3 move = transform.right * x * runSpeed * Time.deltaTime + transform.forward * z * runSpeed * Time.deltaTime;
        characterController.Move(move);

        characterController.Move(velocity * Time.deltaTime);

        correr();
    }

    private void salto()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f;
        }

        if (Input.GetKeyDown("space") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetBool("IsJumping", true);
        }

        if (!isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void correr()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            runSpeed = 25f;
        }
        else
        {
            isSprinting = false;
            runSpeed = 20f;
        }
    }
}
