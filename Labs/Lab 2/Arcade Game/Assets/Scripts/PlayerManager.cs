using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int playerHealth;

    [SerializeField] bool isInvincible;
    [SerializeField] float invincibilityTimer;

    float invincibilityCooldown = 1.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        invincibilityTimer += Time.deltaTime;

        if (playerHealth <= 0) // Destroying player object when their health is 0
        {
            Destroy(this.gameObject);
        }

        if (invincibilityTimer >= invincibilityCooldown) // Invincibility only lasts as long as the invincibility cooldown
        {
            isInvincible = false;
        }
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && isInvincible == false) // Taking damage when touching an enemy
        {
            invincibilityTimer = 0;
            playerHealth--;
            isInvincible = true;
        }

        if (other.gameObject.CompareTag("Health")) // Getting health when picking up a health powerup
        {
            Destroy(other.gameObject);
            playerHealth++;
        }
    }
}
