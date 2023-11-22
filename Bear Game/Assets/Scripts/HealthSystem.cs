using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
 
public class HealthSystem : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioSource audioSource;
 
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        Debug.Log("Health: " + health);

        // Play damage sound
        if (audioSource && damageSound)
        {
            audioSource.PlayOneShot(damageSound);
        }

        animator.SetTrigger("damage");
        
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (health <= 0)
        {
            Die();
            RestartLevel();
        }
    }
 
    void Die()
    {
        animator.SetTrigger("death");
        
        if (deathSound != null && audioSource !=null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        float deathAnimationLength = GetAnimationLength("death");
        Invoke(nameof(RestartLevel), deathAnimationLength);
    }

    void RestartLevel()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    float GetAnimationLength(string animationName)
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        foreach (AnimatorClipInfo info in clipInfo)
        {
            if (info.clip.name == animationName)
            {
                return info.clip.length;
            }
        }

        return 0f; 
    }
    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
 
    }
}