using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource attackSound2;
    [SerializeField] private AudioSource attackSound3;

    
    //References
    public Transform attackPoint;
    private Animator anim;
    private Health enemyHealth;
    private EnemyPatrol enemyPatrol;
    private float cooldownTimer = Mathf.Infinity;
    private HeroKnight hero;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private int m_facingDirection = 1;
    
    private void Awake()
    {
        hero = GetComponent<HeroKnight>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        enemyHealth = GetComponent<Health>();

    }
void Update()
    {

        m_timeSinceAttack += Time.deltaTime;

        // Swap direction of sprite depending on walk direction

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            attackPoint.transform.position = new Vector3(hero.transform.position.x +1 , attackPoint.transform.position.y, attackPoint.transform.position.z);
        }

        else if (inputX < 0)
        {
            attackPoint.transform.position = new Vector3(hero.transform.position.x - 1, attackPoint.transform.position.y, attackPoint.transform.position.z);
        }

        //Attack
        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.5f && !HeroKnight.Instance.Rolling && StaminaBar.Instance.currentStamina>20)
        {
            m_currentAttack++;
            StaminaBar.Instance.UseStamina(10);
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;
            
           
            Attack();
           
        }
    }

    private void Attack()
    {
        switch (m_currentAttack)
        {
            case 1:
                attackSound.Play();
                break;
            case 2:
                attackSound2.Play();
                break;
            case 3:
                attackSound3.Play();
                break;
        }

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        anim.SetTrigger("Attack" + m_currentAttack);
        
        // Reset timer
        m_timeSinceAttack = 0.0f;
       
    }
    private void DealDamage()
    {
        Collider2D[] hitEnnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange *m_facingDirection, enemyLayer);
        foreach (Collider2D enemy in hitEnnemies)
        {
           enemy.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange* m_facingDirection);          
    }

}
