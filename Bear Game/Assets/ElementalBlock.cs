using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBlock : MonoBehaviour
{

    public char weakness = 'a';

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Rock")) // checks if a rock hit it
        {
            if (collision.transform.GetComponent<ThrownRock>().effect == weakness) // checks if it matches this wall's weakness
            {
                Destroy(gameObject); // destroys the wall if it does
            }
        }
    }
}
