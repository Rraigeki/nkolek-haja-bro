using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] protected float dmg;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().TakeDamage(dmg);
        }
    }
}
