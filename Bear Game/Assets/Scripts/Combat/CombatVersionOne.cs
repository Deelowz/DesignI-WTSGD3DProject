using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CombatVersionOne : MonoBehaviour
{
    //-----------------Health Stuff-------------------
    public int health = 15;
    public UnityEngine.UI.Slider healthSlider;
    public TMP_Text healthText;

    public UnityEngine.UI.Slider armorSlider;
    public int armor = 0;

    //-----------------Sword and Combat Stuff---------------------
    public bool isSwinging = false;
    public bool isRecoiling = false;

    public GameObject[] sword;
    public int swordIndex = 0;

    public ItemController swordSlot;
    public ItemController rockSlot;

    public InventoryManagerTwo inventoryManagerTwo;

    //------------------Rock Throw Things---------------------------
    [SerializeField] private LayerMask groundMask;

    public Camera birdEyeCamera;

    [Header("References")]
    //public Transform cam;
    public Transform attackPoint;
    public GameObject[] objectToThrow;

    [Header("Settings")]
    //public int totalThrows;
    public float throwCooldown;
    public float swingCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    public bool readyToThrow;

    //--------------------------------------------------------------



    void Start()
    {
        readyToThrow = true;
    }



    // Update is called once per frame
    void Update()
    {
        // Aiming
        Aim();

        // Sword Swinging
        if (Input.GetMouseButtonDown(0)) // Player left clicks, swinging the sword.
        {
            if (!isSwinging && !isRecoiling) // Makes sure the player isn't in the middle of attacking or getting hit.
            {
                if (swordSlot.Item.id == 21) // The sword is the Blue Marlin Sabre
                {
                    swordIndex = 0;
                }
                else if (swordSlot.Item.id == 22) // The sword is the Eel Sword
                {
                    swordIndex = 1;
                }
                else if (swordSlot.Item.id == 23) // The sword is Chloe's Sword
                {
                    swordIndex = 2;
                }
                else // No sword equipped
                {
                    swordIndex = 3;
                    GetComponent<Animator>().Play("horizontal attack");
                }

                isSwinging = true; // Sets isSwinging to true so the player cannot attack until the animation is over.
                //sword[swordIndex].GetComponent<Animator>().Play("Swing"); // Plays the animation of the selected sword regardless of if it can hit something.
                //sword[swordIndex].GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f); // Changes the pitch of the sound slightly to add variety.
                //sword[swordIndex].GetComponent<AudioSource>().Play(); // Plays the sword's swing sound effect.

                Invoke(nameof(ResetSwordSwing), swingCooldown);
            }
        }


        // Rock Throwing Check
        if (Input.GetMouseButtonDown(1) && readyToThrow && inventoryManagerTwo.equippedSlot[3].GetComponent<ItemController>().Item.amount > 0 && inventoryManagerTwo.equippedSlot[3].GetComponent<ItemController>().Item.type == 3) // Player right clicks, throwing a rock. It checks if they can and have the rocks.
        {
            ThrowRock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Spike"))
        {
            TakeDamage(5);
        }
    }

    public void TakeDamage(int damage)
    {
        // health -= damage;
        //healthSlider.value = health;
        healthSlider.value -= (damage-armor);
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }

    public void HealthBoost(int boost)
    {
        healthSlider.maxValue = health + boost;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }

    public void HealHealth(int health)
    {
        healthSlider.value += health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
    }

    public void ThrowRock()
    {
        var (success, position) = GetMousePosition();

        if (success)
        {
            //-----------------------Instantiates and throws the rock--------------------------------------------------------------
            readyToThrow = false;

            GameObject projectile = Instantiate(objectToThrow[rockSlot.Item.id], attackPoint.position, attackPoint.rotation);
            Vector3 forceDirection = calculateRockVelocity(transform.position, position);

            projectile.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);

            //-----------------------Updates inventory information related to rock-------------------------------------------------


            rockSlot.Item.amount--; // lowers equipped rock amount

            if (rockSlot.Item.amount == 0) // delete rock from inventory
            {
                rockSlot.Item.amount = 1;
                inventoryManagerTwo.selectedButton = inventoryManagerTwo.equippedSlot[3];
                inventoryManagerTwo.DropItem();
            }

            inventoryManagerTwo.UpdateSlots();

            //-------------------------------------------Cooldown------------------------------------------------------------------

            Invoke(nameof(ResetThrow), throwCooldown);

            Debug.Log("Rock throw called");
        }
    }

    Vector3 calculateRockVelocity(Vector3 source, Vector3 target)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = 30 * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        return velocity * direction.normalized;
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = birdEyeCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // ray hit something, return position
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // ray did not hit anything
            return (success: false, position: Vector3.zero);
        }
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // calculate direction.
            var direction = position - transform.position;

            // ignore height difference.
            direction.y = 0;

            // makes the transform look in the direction.
            transform.forward = direction;
        }
    }


    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void ResetSwordSwing()
    {
        isSwinging = false;
    }
}
