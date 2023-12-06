using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CasterEnemy : MonoBehaviour
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

    public GameObject player;
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



    //Caster Stuff

    public Transform attackPoint;
    public GameObject objectToThrow;

    public float throwTimer = 0;


    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        player = GameObject.FindGameObjectWithTag("Player");


    }

    void Update()
    {
        if (!isDead)
        {
            if (player == null)
            {
                Debug.LogError("Agent or player is null.");
                return;
            }

            if (player == null)
                return;

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= aggroRange)
            {
                Aim();

                if (damageTimer <= 0)
                {
                    damageTimer = 4;
                    Invoke("ThrowRock", 1);
                }
            }
        }

        if (throwTimer > 0)
        {
            throwTimer -= Time.deltaTime;
        }

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

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

    Vector3 calculateRockVelocity(Vector3 source, Vector3 target)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = 15 * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        return velocity * direction.normalized;
    }

    public void ThrowRock()
    {
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);
        Vector3 forceDirection = calculateRockVelocity(transform.position, player.transform.position);

        projectile.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
        
    }

    private void Aim()
    {
        // calculate direction.
        var direction = player.transform.position - transform.position;

        // ignore height difference.
        direction.y = 0;

        // makes the transform look in the direction.
        transform.forward = direction;
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