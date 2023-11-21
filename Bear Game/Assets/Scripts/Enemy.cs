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
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;
    bool isAttacking;

    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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

        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                if (healthSlider.value <= healthSlider.minValue)
                {
                    Die();
                }
                else
                {
                    animator.SetTrigger("attack");
                    timePassed = 0;
                    isAttacking = true;
                }
            }
        }

        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }

        newDestinationCD -= Time.deltaTime;
        transform.LookAt(player.transform);
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
            Destroy(collision.gameObject);
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
