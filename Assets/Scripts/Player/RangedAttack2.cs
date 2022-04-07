using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack2 : MonoBehaviour
{
    public Transform fireposition;
    public GameObject projectile;
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

        if (Input.GetKeyDown("v"))
        {      
            anim.SetTrigger("Ranged");
        }


    }

    private void RangedAttack1()
    {
        
        Instantiate(projectile, fireposition.position, fireposition.rotation);
    }

}