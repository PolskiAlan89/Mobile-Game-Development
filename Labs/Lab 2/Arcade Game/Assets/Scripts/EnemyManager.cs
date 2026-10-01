using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public GameObject healthPickup;

    Rigidbody2D rb2DEnemy;
    [SerializeField] float enemySpeed;
    float enemyJumpForce = 7;
    bool enemyCanJump = false;

    UIManager uiManager;
    PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2DEnemy = GetComponent<Rigidbody2D>();
        uiManager = FindFirstObjectByType<UIManager>();
        player = PlayerManager.instance?.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x < player.transform.position.x) // Making enemy follow player depending if they're to the left or right of the enemy
        {
            rb2DEnemy.linearVelocityX = enemySpeed;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (this.gameObject.transform.position.x > player.transform.position.x)
        {
            rb2DEnemy.linearVelocityX = -enemySpeed;
            GetComponent <SpriteRenderer>().flipX = true;
        }

        if (this.gameObject.transform.position.y + 1 < player.transform.position.y && enemyCanJump == true) // Enemy can jump if player is above them
        {
            rb2DEnemy.linearVelocityY += enemyJumpForce;
            enemyCanJump = false;
        }

        if (PlayerManager.instance.playerHealth <= 0) // All enemies disappear once player is dead
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other) // Ground check
    {
        if (other.gameObject.CompareTag("Ground") && rb2DEnemy.linearVelocityY == 0)
        {
            enemyCanJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) // Ground check
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            enemyCanJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hitbox")) // Enemy dies once player hits them
        {
            Destroy(this.gameObject);
            uiManager.EnemyKill();
            if (PlayerManager.instance.playerHealth == 1) // Drop health if player is low
            {
                Instantiate(healthPickup, transform.position, Quaternion.identity);
            }
        }
    }
}
