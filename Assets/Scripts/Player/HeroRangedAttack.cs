using UnityEngine;

public class HeroRangedAttack : MonoBehaviour
{
    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private float attackCooldown;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;   

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown("v") && cooldownTimer > attackCooldown && StaminaBar.Instance.currentStamina > 20)
        {
            StaminaBar.Instance.UseStamina(20);
            anim.SetTrigger("Ranged");
            
        }

    }

    private void RangedAttack()
    {
        
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
