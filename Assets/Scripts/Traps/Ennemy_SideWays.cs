
using UnityEngine;

public class Ennemy_SideWays : MonoBehaviour
{
    [SerializeField] private float dmg;
    [SerializeField] private float speed;
    [SerializeField] private float movementDistance;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private float zspeed = 5.0f;

    private void Awake()
    {
        leftEdge  = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        zspeed += Time.deltaTime * 500;





        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, zspeed);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, -zspeed);
            }
            else
            { 
                movingLeft = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player")
        {
            collision.GetComponent<PlayerStats>().TakeDamage(dmg);
        }
    }

}
