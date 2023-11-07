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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                }

                isSwinging = true; // Sets isSwinging to true so the player cannot attack until the animation is over.
                sword[swordIndex].GetComponent<Animator>().Play("Swing"); // Plays the animation of the selected sword regardless of if it can hit something.
                sword[swordIndex].GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f); // Changes the pitch of the sound slightly to add variety.
                sword[swordIndex].GetComponent<AudioSource>().Play(); // Plays the sword's swing sound effect.
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Player right clicks, throwing a rock.
        {
            if (rockSlot.Item.id != 0) // Makes sure a rock is equipped.
            {
                ThrowRock(rockSlot.Item);
            }
        }
    }

    public void ThrowRock(Item rock)
    {
        Instantiate(rock);
    }
}
