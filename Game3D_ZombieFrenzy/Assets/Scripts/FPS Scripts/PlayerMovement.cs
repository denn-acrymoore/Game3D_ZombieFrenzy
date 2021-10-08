using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController charController;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;

    [Header("Movement Settings")]
    [SerializeField] float speed = 8f;
    [SerializeField] float gravity = -19.62f;   // Double the gravity value because it's too slow
    [SerializeField] float groundMinDistance = 0.2f;

    Vector3 gravityVelocity;
    bool isGrounded;

    void Update()
    {
        if (GameManagerScript.isPlayerAlive)
        {
            FPSMovement();
        }
    }

    void FPSMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundMinDistance
            , groundMask);

        // Reset our velocity if we touch the ground:
        if (isGrounded && gravityVelocity.y < 0)
        {
            // We specify it to be -2 so that the player is guaranteed to be on the ground
            // When the isGrounded = true
            gravityVelocity.y = -2f;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move player based on input:
        Vector3 move = transform.right * h + transform.forward * v;
        move = move.normalized;

        charController.Move(move * speed * Time.deltaTime);

        // Simulate gravity using code:
        // NOTE: We multiply time twice because that's how gravity works in physics
        // - vt = v0 + g * t
        // - delta v = g * t

        gravityVelocity.y += gravity * Time.deltaTime;

        // - s = v * t
        // NOTE: This isn't very accurate to physics, but it works!!! 
        charController.Move(gravityVelocity * Time.deltaTime);
    }
}
