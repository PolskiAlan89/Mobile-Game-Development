using System.Threading;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject meleeHitbox;
    public GameObject lightningHitbox;

    [SerializeField] bool canAttack;
    [SerializeField] bool canShoot;
    [SerializeField] float meleeTimer;
    [SerializeField] float lightningTimer;

    float meleeCooldown = 0.5f;
    float lightningVisualCooldown = 0.5f;
    float lightningAttackCooldown = 5.0f;
    float meleeHitboxOffset = 1.25f;
    float lightningHitboxOffset = 2.7f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canAttack = true;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        meleeTimer += Time.deltaTime;
        lightningTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && canAttack == true && GetComponent<SpriteRenderer>().flipX == false) // Code for attacking melee attack
        {
            meleeTimer = 0;
            Vector2 hitboxPosition = new(this.gameObject.transform.position.x + meleeHitboxOffset, this.gameObject.transform.position.y);
            meleeHitbox.transform.position = hitboxPosition;
            meleeHitbox.SetActive(true);
            canAttack = false;
        }
        else if (Input.GetButtonDown("Fire1") && canAttack == true && GetComponent<SpriteRenderer>().flipX == true)
        {
            meleeTimer = 0;
            Vector2 hitboxPosition = new(this.gameObject.transform.position.x - meleeHitboxOffset, this.gameObject.transform.position.y);
            meleeHitbox.transform.position = hitboxPosition;
            meleeHitbox.SetActive(true);
            canAttack = false;
        }

        if (Input.GetButtonDown("Fire2") && canShoot == true && GetComponent<SpriteRenderer>().flipX == false) // Code for attacking with lightning attack
        {
            lightningTimer = 0;
            Vector2 lightningHitboxPosition = new(this.gameObject.transform.position.x + lightningHitboxOffset, this.gameObject.transform.position.y);
            lightningHitbox.transform.position = lightningHitboxPosition;
            lightningHitbox.SetActive(true);
            canShoot = false;
        }
        else if (Input.GetButtonDown("Fire2") && canShoot == true && GetComponent<SpriteRenderer>().flipX == true) // Code for attacking with lightning attack
        {
            lightningTimer = 0;
            Vector2 lightningHitboxPosition = new(this.gameObject.transform.position.x - lightningHitboxOffset, this.gameObject.transform.position.y);
            lightningHitbox.transform.position = lightningHitboxPosition;
            lightningHitbox.SetActive(true);
            canShoot = false;
        }
        if (meleeTimer >= meleeCooldown) // Being able to attack again once melee timer is higher than melee cooldown
        {
            meleeHitbox.SetActive(false);
            canAttack = true;
        }

        if (lightningTimer >= lightningVisualCooldown) // Making the lightning attack not last forever
        {
            lightningHitbox.SetActive(false);
        }

        if (lightningTimer >= lightningAttackCooldown) // Can attack with lightning attack again once timer is higher than cooldown
        {
            canShoot = true;
        }
    }
}
