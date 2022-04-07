using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject[] trap1;
    [SerializeField] private float yspeed;
    private bool isTriggered = false;
    void Start()
    {
 
    }

    void Update()
    {
        
        if (isTriggered)
        {
            trap1[0].transform.position = new Vector3(trap1[0].transform.position.x, trap1[0].transform.position.y + yspeed * Time.deltaTime,
            transform.position.z);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           isTriggered = true;
        }
    }
}
