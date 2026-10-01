using System.Runtime.CompilerServices;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2DPlayer;
    [SerializeField] bool canJump = false;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference jumpAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2DPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveAction.action.ReadValue<Vector2>(); // Allow player to move left and right
        rb2DPlayer.linearVelocityX = move.x * speed;

        if (rb2DPlayer.linearVelocityX < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (canJump == true && jumpAction.action.WasPressedThisFrame()) // Jump code
        {
            rb2DPlayer.linearVelocityY += jumpForce;
            canJump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other) // Making player able to jump only if they are touching the ground
    {
        if (other.gameObject.CompareTag("Ground") && rb2DPlayer.linearVelocityY == 0)
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) // Make player unable to jump if they fall off ground
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
