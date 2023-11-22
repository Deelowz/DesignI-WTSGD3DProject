using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioSource audioSource;
 
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        // Play damage sound
        if (audioSource && damageSound)
        {
            audioSource.PlayOneShot(damageSound);
        }

        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }
 
    void Die()
    {
        animator.SetTrigger("death");
        Destroy(this.gameObject);
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
 
    }
}