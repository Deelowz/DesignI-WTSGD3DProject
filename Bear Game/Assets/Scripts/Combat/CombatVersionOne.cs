using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVersionOne : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSwinging = false;
    public bool isRecoiling = false;

    public GameObject[] sword;
    public int swordIndex = 0;

    public ItemController swordSlot;
    public ItemController rockSlot;

    public GameObject rockPrefab;

    //------------------Rock Throw Things---------------------------

    [SerializeField] private LayerMask groundMask;

    public Camera birdEyeCamera;

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

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

        Aim();

        //if (Input.GetMouseButtonDown(0)) // Player left clicks, swinging the sword.
        //{
        //    if (!isSwinging && !isRecoiling) // Makes sure the player isn't in the middle of attacking or getting hit.
        //    {
        //        if (swordSlot.Item.id == 21) // The sword is the Blue Marlin Sabre
        //        {
        //            swordIndex = 0;
        //        }
        //        else if (swordSlot.Item.id == 22) // The sword is the Eel Sword
        //        {
        //            swordIndex = 1;
        //        }
        //        else if (swordSlot.Item.id == 23) // The sword is Chloe's Sword
        //        {
        //            swordIndex = 2;
        //        }
        //        else // No sword equipped
        //        {
        //            swordIndex = 3;
        //        }

        //        isSwinging = true; // Sets isSwinging to true so the player cannot attack until the animation is over.
        //        sword[swordIndex].GetComponent<Animator>().Play("Swing"); // Plays the animation of the selected sword regardless of if it can hit something.
        //        sword[swordIndex].GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f); // Changes the pitch of the sound slightly to add variety.
        //        sword[swordIndex].GetComponent<AudioSource>().Play(); // Plays the sword's swing sound effect.
        //    }
        //}
        if (Input.GetMouseButtonDown(1) && readyToThrow && totalThrows > 0) // Player right clicks, throwing a rock. It checks if they can and have the rocks.
        {
            //if (rockSlot.Item.id != 0) // Makes sure a rock is equipped.
            //{
                ThrowRock();
            //}
        }
    }

    public void ThrowRock()
    {
        readyToThrow = false;

        // instantiates the throw object
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);

        // get the object's rigidbody
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();



        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }



        // adds force to it
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // cooldown
        Invoke(nameof(ResetThrow), throwCooldown); 
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
}
