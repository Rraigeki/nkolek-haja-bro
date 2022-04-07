using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    private Animator m_animator;
    private bool shieldUp = false;
    private int m_facingDirection = 1;


    #region Sigleton
    private static ShieldBlock instance;
    public static ShieldBlock Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ShieldBlock>();
            return instance;
        }
    }
    #endregion
    public bool ShieldUp { get { return shieldUp; } }
    
    private void Awake()
    {
        m_animator = GetComponent<Animator>();

    }

    private void Update()
    { //Block
        if (Input.GetMouseButtonDown(1) && !HeroKnight.Instance.Rolling)
        {
            block();
        }
        else  if (Input.GetMouseButton(1) && !HeroKnight.Instance.Rolling)
        {
            shieldUp = true;
        }
        else
        {
            if (Input.GetMouseButtonUp(1) && !HeroKnight.Instance.Rolling)
                m_animator.SetBool("IdleBlock", false);

            shieldUp = false;
            
        }
    }

    public void block()
    {
        m_animator.SetTrigger("Block");
        m_animator.SetBool("IdleBlock", true);
        shieldUp = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * colliderDistance,
             new Vector3(boxCollider.bounds.size.x , boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
