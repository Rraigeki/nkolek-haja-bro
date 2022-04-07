using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    //enemyInitFace  Left = 1 ,Right = -1  
    [SerializeField] private int enemyInitFace;


    [Header("Enemy")]
    [SerializeField]  private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private  float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField]  float idleDuration;
    static float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator m_animator;

    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
        m_animator.SetInteger("AnimState", 0);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        m_animator.SetInteger("AnimState", 0);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int m_direction)
    {
        idleTimer = 0;
        m_animator.SetInteger("AnimState", 2);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) *m_direction * -enemyInitFace ,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * m_direction * speed ,
            enemy.position.y, enemy.position.z);
    }
}