using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
 
public class Enemy: MonoBehaviour
{
    // Healthbar things
    public UnityEngine.UI.Slider healthSlider;
    public TMP_Text healthText;


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
 
    void Start()
    {
        // Sets healthbar to this script's health
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
 
    // Update is called once per frame
    void Update()
    {
        if (agent == null || player == null)
    {
        // Log an error or return early if agent or player is not set.
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
                animator.SetTrigger("attack");
                timePassed = 0;

                if (Vector3.Distance(player.transform.position, transform.position) <= 2) // Checks if the player is super close. Change this to occur in the middle of the attack swing as an event.
                {
                    player.GetComponent<CombatVersionOne>().TakeDamage(3); // hurts player, 3 as placeholder damage.
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

        if (collision.gameObject.CompareTag("Rock")) // checks if a rock projectile has hit
        {
            TakeDamage(collision.transform.GetComponent<ThrownRock>().damage); // calls the TakeDamage method and sends the rock script's damage
            Destroy(collision.gameObject);
        }
    }

 
    void Die()
    {
        animator.SetTrigger("death");
        StartCoroutine(DestroyAfterDelay(2.0f));
        Destroy(this.gameObject);
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
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the GameObject after the specified delay.
        Destroy(gameObject);
    }
}