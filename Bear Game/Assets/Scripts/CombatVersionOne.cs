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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Gets input.
        {
            if (!isSwinging && !isRecoiling) // Makes sure the player isn't in the middle of attacking or getting hit.
            {
                isSwinging = true; // Sets isSwinging to true so the player cannot attack until the animation is over.
                sword[swordIndex].GetComponent<Animator>().Play("Swing"); // Plays the animation of the selected sword regardless of if it can hit something.
                sword[swordIndex].GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f); // Changes the pitch of the sound slightly to add variety.
                sword[swordIndex].GetComponent<AudioSource>().Play(); // Plays the sword's swing sound effect.
            }
        }
    }
}
