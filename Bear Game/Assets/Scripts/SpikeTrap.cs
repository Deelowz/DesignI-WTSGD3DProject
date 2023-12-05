using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip trap;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Volume of sound
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        transform.GetComponent<Animator>().Play("Spikes");
        if (audioSource != null && trap != null)
        {
            audioSource.PlayOneShot(trap);
        }
    }
}
