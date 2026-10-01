using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2DPlayer;
    [SerializeField] bool canJump = false;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2DPlayer = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Moving left and right code
        rb2DPlayer.linearVelocityX = moveHorizontal * speed;

        if (rb2DPlayer.linearVelocityX < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (canJump == true && Input.GetButtonDown("Jump")) // Jump code
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
