using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 6.0f;
    [SerializeField] float      m_jumpForce = 9.5f;
    [SerializeField] float      m_rollForce = 8.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] LayerMask  wallLayer;
    [SerializeField] LayerMask  groundLayer;

    private BoxCollider2D       boxCollider;
    public  Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    public  PlayerStats         m_playerStat;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    public  static bool         m_dead = false;
    private int                 m_facingDirection = 1;
    private float               m_delayToIdle = 0.0f;
    private float               wallJumpCD;
    private bool                wallJump;
    private float               inputX;

    public bool Rolling { get { return m_rolling; } }

    #region Sigleton
    private static HeroKnight instance;
    public static HeroKnight Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<HeroKnight>();
            return instance;
        }
    }
    #endregion


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {

        // -- Handle input and movement --
        inputX = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (inputX > 0.01f) 
        { 
            transform.localScale = new Vector3(1.4f, 1.65f, 1);
            m_facingDirection = 1;
        }
        else if (inputX < -0.01f)
        {
            transform.localScale = new Vector3(-1.4f, 1.65f, 1);
            m_facingDirection = -1;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            AE_ResetRoll();
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

         // Move
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

       
        //Wall Slide
        m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()));
     
        // Wall Jump
        if ((m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State())) 
        {
            AE_ResetRoll();
            if (wallJumpCD > 0.2f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(-m_body2d.velocity.x * inputX, m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                }
            }
            else wallJumpCD += Time.deltaTime;
        }
        //Death
        if (Input.GetKeyDown("e") )
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            PlayerStats.Instance.Kill() ;
            //GetComponent<HeroKnight>().enabled = false;
           
        }
        //Heal 
        else if (Input.GetKeyDown("t"))
                {
            PlayerStats.Instance.Heal(1);

        }
       

      // // Block
      // else if (Input.GetMouseButtonDown(1) )
      // {
      //     m_animator.SetTrigger("Block");
      //     m_animator.SetBool("IdleBlock", true);
      // }

       

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && StaminaBar.Instance.currentStamina > 20)
        {
            StaminaBar.Instance.UseStamina(20);
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }


        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon )
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        //Wall Jump
       /* if (wallJumpCD > 0.2f)
        {
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            if ((m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()))
            {
                m_body2d.gravityScale = 0;
                m_body2d.velocity = Vector2.zero;
            }
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
        }
        else wallJumpCD += Time.deltaTime;*/

    }

    

  

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        m_rolling = false;
    }

    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
