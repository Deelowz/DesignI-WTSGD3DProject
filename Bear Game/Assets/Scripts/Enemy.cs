using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Healthbar things
    public UnityEngine.UI.Slider healthSlider;
    public TMP_Text healthText;

    // When the enemy dies, we play an explosion
    public Transform explosion;

    [SerializeField] int health = 15;
    [SerializeField] GameObject hitVFX;
    //[SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 5f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;
    bool isAttacking;

    AudioSource audioSource;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;

    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (agent == null || player == null)
        {
            Debug.LogError("Agent or player is null.");
            return;
        }

        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= aggroRange)
        {
            // Player is within aggro range, track the player
            if (timePassed >= attackCD)
            {
                if (distanceToPlayer <= attackRange)
                {
                    if (healthSlider.value <= healthSlider.minValue)
                    {
                        Die();
                    }
                    else
                    {
                        animator.SetTrigger("attack");
                        timePassed = 0;

                        if (isAttacking == false)
                        {
                            Invoke("Attack", 1); // waits 1 second to call Attack() since that's when the swing would make contact.
                        }

                        isAttacking = true;


                        if (attackSound != null)
                        {
                            audioSource.PlayOneShot(attackSound);
                        }

                        Invoke("AttackCompleted", 3); // resets the isAttacking so it can attack again.
                    }
                }
            }

            timePassed += Time.deltaTime;

            if (newDestinationCD <= 0)
            {
                newDestinationCD = 0.5f;
                agent.SetDestination(player.transform.position);
            }

            newDestinationCD -= Time.deltaTime;
            transform.LookAt(player.transform);
        }
        else
        {
            // Player is outside aggro range, stop tracking the player
            agent.ResetPath();
            animator.SetFloat("speed", 0f); // Stop the enemy's movement animation
        }
    }

    public void Attack()
    {
        Debug.Log("attempted attack");
        if (Vector3.Distance(player.transform.position, transform.position) <= 2)
        {
            player.GetComponent<CombatVersionOne>().TakeDamage(5);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print(true);
            player = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(collision.transform.GetComponent<ThrownRock>().damage);
            collision.transform.GetComponent<ThrownRock>().Break();
        }
    }

    void Die()
    {
        if (isAttacking)
        {
            // Interrupt the attack animation
            animator.SetTrigger("interruptAttack");
        }

        Debug.Log("Die() method called. Triggering death animation...");
        animator.SetTrigger("death");

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        StartCoroutine(ExplodeAfterAnimation(2.0f)); // Adjust the time delay for explosion as needed
    }

    IEnumerator ExplodeAfterAnimation(float delay)
    {
        float deathAnimationLength = GetAnimationLength("death");
        Debug.Log("Death animation length: " + deathAnimationLength);

        // Wait for the death animation
        yield return new WaitForSeconds(deathAnimationLength);

        // Instantiate vfx explosion after death animation finishes
        if (explosion)
        {
            GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
            Destroy(exploder, 0.1f); // Play for 1-2 frames
        }

        // Destroy the enemy after a short delay
        yield return new WaitForSeconds(delay);

        Debug.Log("Destroying GameObject after death animation and explosion.");
        Destroy(gameObject);
    }



    public void TakeDamage(int damageAmount)
    {
        healthSlider.value -= damageAmount;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
        animator.SetTrigger("damage");

        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (healthSlider.value <= healthSlider.minValue)
        {
            Die();
        }
    }

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyCombat>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyCombat>().EndDealDamage();
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    IEnumerator DestroyAfterAnimation(float delay)
    {
        float deathAnimationLength = GetAnimationLength("death");
        Debug.Log("Death animation length: " + deathAnimationLength);

        yield return new WaitForSeconds(Mathf.Max(deathAnimationLength, delay));

        Debug.Log("Destroying GameObject after death animation.");
        Destroy(gameObject);
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

        return 3.0f;
    }

    public void AttackCompleted()
    {
        if (healthSlider.value <= healthSlider.minValue && isAttacking)
        {
            Die();
        }
        isAttacking = false;
    }
}
