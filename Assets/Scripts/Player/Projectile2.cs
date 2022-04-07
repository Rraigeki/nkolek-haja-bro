using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    [SerializeField] private float projectilespeed;
    [SerializeField] private float damage;
    public GameObject impactEffect;
    private bool hit;
    private Rigidbody2D rigidbody;
    private Animator anim;
    private float lifetime;
    private float direction;
    private BoxCollider2D boxCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * projectilespeed;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (lifetime > 5) Destroy(gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        anim.SetTrigger("explode");
        boxCollider.enabled = false;
        if ( collision.tag == "Enemy")
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            collision.GetComponent<Health>().TakeDamage(damage);
            
        }
        
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
      
    }

    private void Deactivate1()
    {
        Destroy(gameObject);
    }
    
}
