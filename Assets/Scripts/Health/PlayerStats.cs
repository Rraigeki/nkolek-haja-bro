using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;
    private Animator m_animator;
    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion
    [Header("Health")]
    [SerializeField]  private float health;
    [SerializeField]  private float maxHealth;
    [SerializeField]  private float maxTotalHealth;

    [Header("iFrames")]
    [SerializeField]  private float iFrameDuration;
    [SerializeField]  private int numberOfFlashes;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private SpriteRenderer spriteRend;
    private HeroKnight heroknight;
    private EnemyPatrol enemyPatrol;
    private MeleeEnemy enemyMelee;
    private Rigidbody2D rigBody;
    private ShieldBlock block;




    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    private void Awake()
    { 
        m_animator = GetComponent<Animator>();
        heroknight = GetComponent<HeroKnight>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        enemyMelee = GetComponent<MeleeEnemy>();
        rigBody = GetComponent<Rigidbody2D>();
        block = GetComponentInParent<ShieldBlock>();
        spriteRend = GetComponent<SpriteRenderer>();


    }
    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }
    
    public void Kill()
    {
        health = 0;
        ClampHealth();
        m_animator.SetTrigger("Death");

        //Deactivate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = false;
        rigBody.simulated = false;
    }

    public void TakeDamage(float dmg)
    {
        if (ShieldBlock.Instance.ShieldUp == false )
        {   if (HeroKnight.Instance.Rolling)
            {
                health -= dmg;
                ClampHealth();
                StartCoroutine(Invunerability());
            }
            else
            { 
                health -= dmg;
                ClampHealth();
                if (health > 0)
                {
                    m_animator.SetTrigger("Hurt");
                   
                }

                else
                    Kill();
            }
        }
        else
            ShieldBlock.Instance.block();

    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }
    
    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
