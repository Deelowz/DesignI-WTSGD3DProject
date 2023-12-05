using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;
    public Transform explosion;

    [SerializeField] int health = 15;
    [SerializeField] GameObject hitVFX;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 5f;

    public float damageTimer = 2;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 2.5f;
    bool isAttacking;
    public bool isDead = false;

    AudioSource audioSource;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] string idleTrigger = "idleTrigger"; 


    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = 0.2f;
    }

    void Update()
    {
        if (!isDead)
        {
            if (agent == null || player == null)
            {
                Debug.LogError("Agent or player is null.");
                return;
            }

            if (player == null)
                return;

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= aggroRange)
            {
                if (timePassed >= attackCD)
                {
                    if (distanceToPlayer <= attackRange)
                    {
                        if (healthSlider.value <= healthSlider.minValue)
                        {
                            if (!isDead)
                            {
                                Die();
                                isDead = true;
                            }
                        }
                        else
                        {
                            animator.SetTrigger("attack");
                            timePassed = 0;

                            if (!isAttacking)
                                Invoke("Attack", 1);

                            isAttacking = true;

                            if (attackSound != null)
                                audioSource.PlayOneShot(attackSound);

                            Invoke("AttackCompleted", 3);
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
                agent.ResetPath();
                animator.SetTrigger(idleTrigger);
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            aggroRange = 0;
            attackRange = 0;
        }

        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        Debug.Log("attempted attack");
        if (Vector3.Distance(player.transform.position, transform.position) <= 2)
            player.GetComponent<CombatVersionOne>().TakeDamage(5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = collision.gameObject;

        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(collision.transform.GetComponent<ThrownRock>().damage);
            collision.transform.GetComponent<ThrownRock>().Break();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "Spike":
            case "BearClaw":
                TakeDamage(5);
                break;
            case "BlueMarlin":
                TakeDamage(10);
                break;
            case "EelSword":
                TakeDamage(7);
                break;
            case "OtherSword":
                TakeDamage(12);
                break;
        }
    }

    void Die()
    {
        animator.Play("death");

        if (isAttacking)
            animator.SetTrigger("interruptAttack");

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        StartCoroutine(ExplodeAfterAnimation(0.5f));
    }

    IEnumerator ExplodeAfterAnimation(float delay)
    {
        float deathAnimationLength = GetAnimationLength("death");

        yield return new WaitForSeconds(deathAnimationLength);

        if (explosion)
        {
            GameObject exploder = ((Transform)Instantiate(explosion, transform.position, transform.rotation)).gameObject;
            Destroy(exploder, 0.1f);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageTimer > 0)
            return;

        healthSlider.value -= damageAmount;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
        damageTimer = 1;

        if (damageSound != null)
            audioSource.PlayOneShot(damageSound);

        if (healthSlider.value <= healthSlider.minValue)
        {
            Die();
            isDead = true;
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    public void AttackCompleted()
    {
        if (healthSlider.value <= healthSlider.minValue && isAttacking)
            Die();
        isAttacking = false;
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
}
