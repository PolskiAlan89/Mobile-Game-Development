using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public GameObject meleeHitbox;
    public GameObject lightningHitbox;

    [SerializeField] bool canAttack;
    [SerializeField] bool canShoot;
    [SerializeField] float meleeTimer;
    [SerializeField] float lightningTimer;
    [SerializeField] InputActionReference fireAction;
    [SerializeField] InputActionReference lightningAction;

    float meleeCooldown = 0.5f;
    float lightningVisualCooldown = 0.5f;
    float lightningAttackCooldown = 5.0f;
    float meleeHitboxOffset = 1.25f;
    float lightningHitboxOffset = 2.7f;

    private void OnEnable()
    {
        fireAction.action.Enable();
        lightningAction.action.Enable();
    }

    private void OnDisable()
    {
        fireAction.action.Disable();
        lightningAction.action.Disable();
    }

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

        if (fireAction.action.WasPressedThisFrame() && canAttack == true && GetComponent<SpriteRenderer>().flipX == false) // Code for attacking melee attack
        {
            meleeTimer = 0;
            Vector2 hitboxPosition = new(this.gameObject.transform.position.x + meleeHitboxOffset, this.gameObject.transform.position.y);
            meleeHitbox.transform.position = hitboxPosition;
            meleeHitbox.SetActive(true);
            canAttack = false;
        }
        else if (fireAction.action.WasPressedThisFrame() && canAttack == true && GetComponent<SpriteRenderer>().flipX == true)
        {
            meleeTimer = 0;
            Vector2 hitboxPosition = new(this.gameObject.transform.position.x - meleeHitboxOffset, this.gameObject.transform.position.y);
            meleeHitbox.transform.position = hitboxPosition;
            meleeHitbox.SetActive(true);
            canAttack = false;
        }

        if (lightningAction.action.WasPerformedThisFrame() && canShoot == true && GetComponent<SpriteRenderer>().flipX == false) // Code for attacking with lightning attack
        {
            lightningTimer = 0;
            Vector2 lightningHitboxPosition = new(this.gameObject.transform.position.x + lightningHitboxOffset, this.gameObject.transform.position.y);
            lightningHitbox.transform.position = lightningHitboxPosition;
            lightningHitbox.SetActive(true);
            canShoot = false;
        }
        else if (lightningAction.action.WasPerformedThisFrame() && canShoot == true && GetComponent<SpriteRenderer>().flipX == true) // Code for attacking with lightning attack
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
