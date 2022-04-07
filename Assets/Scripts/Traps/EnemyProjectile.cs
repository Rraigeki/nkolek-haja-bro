using UnityEngine;

public class EnemyProjectile : EnemyDamage //Will dmg the player evertime it touches 
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;
    private bool hit;
   

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;  
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        base.OnTriggerEnter2D(collision); //Execute logic from parent 
        if (anim != null)
            anim.SetTrigger("explode"); // when the object is a firebal explode it 
        else
            gameObject.SetActive(false); // when this hits anyobject desactivate it
        coll.enabled = false;
        Deactivate();
        
    }
    private void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}
