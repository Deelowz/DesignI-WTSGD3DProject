using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownRock : MonoBehaviour
{
    public int damage = 0;
    public char effect = 'n';


    public void Break()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        Invoke("Destroy", 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
