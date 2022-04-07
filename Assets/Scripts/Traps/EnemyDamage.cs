using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float dmg;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().TakeDamage(dmg);
        }
    }
}
