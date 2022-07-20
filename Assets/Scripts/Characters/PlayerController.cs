using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterStats playerStats;     //Player������

    [Header("Player���")]

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer sprite;
    private Transform groundCheck;                      //������Transform

    private PolygonCollider2D swordAttackCheckBox1;          //sword�������collider1
    private PolygonCollider2D swordAttackCheckBox2;          //sword�������collider2
    private PolygonCollider2D swordAttackCheckBox3;          //sword�������collider3

    [SerializeField] private LayerMask groundLayer;     //������ͼ��

    [Header("�������")]
    public float horizontal;            //ˮƽ���룺A/D <-/->
    public bool jumpPressed;            //��Ծ�����£�Space
    public bool rollPressed;            //���������£�C

    [Header("��������")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int baseDefence;     //��������
    [SerializeField] private int currentDefence;  //��ǰ����

    [Header("�ƶ�����")]
    [SerializeField] private float speed;               //��ǰ�ٶ�
    [SerializeField] private float runSpeed;            //�ܲ��ٶ�
    [SerializeField] private float jumpForce;           //��Ծ��
    [SerializeField] private int maxAirJumpNum;         //����������Ծ����
    [SerializeField] private int currentAirJumpNum;     //��ǰ���п���Ծ����
    [SerializeField] private float rollSpeed;           //�����ٶ�
    [SerializeField] private float climbSpeed;          //�����ٶ�

    [Header("��������")]
    [SerializeField] private float attackRange;         //������Χ
    [SerializeField] private float coolDown;            //��ȴʱ��
    [SerializeField] private float lastAttackTime;      //�ϴι���ʱ��
    [SerializeField] private int maxDamage;             //����˺�
    [SerializeField] private int minDamage;             //��С�˺�
    [SerializeField] private float criticalMultiplier;    //��������
    [SerializeField] private float criticalChance;      //������
    [SerializeField] private bool isCritical;           //�Ƿ񱩻�

    [SerializeField] private int coinNumber = 0;        //�������

    [Header("״̬����")]
    [SerializeField] private bool isAggressive;         //�Ƿ��й����ԣ��Ƿ��������������жϹ��������Ƿ������
    [SerializeField] private bool onGround;             //�Ƿ��ڵ���

    [Header("���˲���")]
    [SerializeField] private float hurtTime = 0.4f;     //���˳���ʱ��
    private Color originalColor;                        //ԭʼ��ɫ
    [SerializeField] private Material originaMaterial;  //ԭʼ����
    [SerializeField] private Material hurtMaterial;     //���˲���

    //��϶����л�
    public bool isHurt { get; set; }
    public bool isJump { get; set; }
    public bool isFall { get; set; }
    public bool isIdle { get; set; }
    public bool isRoll { get; set; }
    public bool isDead { get; set; }

    protected override void Awake()     //��д����Singleton��Awake
    {
        base.Awake();

        swordAttackCheckBox1 = transform.GetChild(1).GetComponent<PolygonCollider2D>();
        swordAttackCheckBox2 = transform.GetChild(2).GetComponent<PolygonCollider2D>();
        swordAttackCheckBox3 = transform.GetChild(3).GetComponent<PolygonCollider2D>();

        groundCheck = transform.GetChild(0).GetComponent<Transform>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<CharacterStats>();

        originalColor = sprite.color;
        originaMaterial = sprite.material;
        //hurtMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/2D Platformer Tileset/Simple UI Pack/Font/Font Material.mat", typeof(Material)) as Material;

        InitStats();    //��ʼ��������
    }

    //��ʼ��������
    void InitStats()
    {
        maxHealth = playerStats.MaxHealth;
        currentHealth = playerStats.CurrentHealth;
        baseDefence = playerStats.BaseDefence;
        currentDefence = playerStats.CurrentDefence;

        runSpeed = playerStats.characterData.runSpeed;
        speed = runSpeed;
        jumpForce = playerStats.characterData.jumpForce;
        maxAirJumpNum = playerStats.characterData.maxAirJumpNum;
        rollSpeed = playerStats.characterData.rollSpeed;
        climbSpeed = playerStats.characterData.climbSpeed;

        coinNumber = playerStats.characterData.coinNumber;

        attackRange = playerStats.attackData.attackRange;
        coolDown = playerStats.attackData.coolDown;
        maxDamage = playerStats.attackData.maxDamage;
        minDamage = playerStats.attackData.minDamage;
        criticalMultiplier = playerStats.attackData.criticalMultiplier;
        criticalChance = playerStats.attackData.criticalChance;
        lastAttackTime = -1;
    }

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayer(playerStats);   //��Player�����ݴ���GameManager
    }

    void Start()
    {
        SaveManager.Instance.LoadPlayerData();  //����Player������
    }

    private void Update()
    {
        isDead = playerStats.CurrentHealth == 0;    //��ǰѪ��Ϊ0������

        if (isDead) //Player����
        {
            Dead(); //����
        }
        else if (!SceneUI.Instance.pause) 
        {
            if (lastAttackTime >= 0) lastAttackTime -= Time.deltaTime;

            Attack();   //����
            
            if (!isHurt)
            {
                Jump();
                //Roll();
            }
               
        }
    }

    void FixedUpdate()
    {
        if (!isDead && !SceneUI.Instance.pause)
        {
            PhysicCheck();

            if (!isHurt) 
                Move();
        }
    }

    /// <summary>
    /// ������Player�������һЩ����
    /// </summary>
    private void Dead()
    {
        animator.SetBool("dead", isDead);
        GameManager.Instance.NotifyObservers();     //֪ͨ����Enemy ��Ϸ����
        GameManager.Instance.GameOver();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void PhysicCheck()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);  //������
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            rigidbody2d.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rigidbody2d.velocity.y);   //�ƶ�
            transform.localScale = new Vector3(horizontal > 0 ? 1 : -1, 1, 1);  //���ҷ�תͼƬ
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Roll()
    {
        rollPressed = Input.GetKeyDown(KeyCode.C);

        isRoll = onGround && rollPressed;   

        //���޸�
        //if(rollPressed)
        //    rigidbody2d.velocity = new Vector2(transform.localScale.x * rollSpeed * Time.deltaTime, rigidbody2d.velocity.y);    //���泯�ķ����ƶ�
        
    }

    /// <summary>
    /// ��Ծ
    /// </summary>
    private void Jump()
    {
        jumpPressed = Input.GetButtonDown("Jump");

        //��ͨ��
        if (onGround && jumpPressed)     //�ڵ��� ������Ծ��
        {
            speed = runSpeed * 0.9f;    //�ڿ����ƶ��ٶȼ�С 
            rigidbody2d.velocity = Vector2.up * jumpForce;
            isJump = true;
        }
        else if(!onGround && rigidbody2d.velocity.y < 0.1f)  //���ڵ��� ����ʱ
        {
            isJump = false;
            isFall = true;
        }

        //�����
        if(onGround && rigidbody2d.velocity.y < 0.1f)   //�ڵ��� û������
        {
            speed = runSpeed;
            currentAirJumpNum = maxAirJumpNum;
        }
        else if (!onGround && jumpPressed && currentAirJumpNum > 0)     //���ڵ��� ��Ծ ���п���Ծ����>0
        {
            speed = runSpeed * 0.9f;    //�ڿ����ƶ��ٶȼ�С 
            rigidbody2d.velocity = Vector2.up * jumpForce;
            isJump = true;
            currentAirJumpNum--;
        }

        if (isJump && onGround)     //��Ծʱ �ڵ��棺��Ծ��ߵ�ʱ�պ����ڵ���
        {
            if(rigidbody2d.velocity.y < 0.1f)
            {
                isJump = false;
                isFall = true;
            }
        }
        else if(onGround)   
        {
            isFall = false;
            isIdle = true;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        if (Input.GetKey(KeyCode.J))
        {
            //�ж��Ƿ񱩻�
            isCritical = Random.value < criticalChance;      //[0,1]���ȡֵ��ȡֵС�ڱ����ʵĸ��ʸպ�Ϊ������
            playerStats.isCritical = isCritical;

            if (lastAttackTime < 0)     //��ȴ����
            {
                animator.SetBool("critical", isCritical);   //������������������

                //�������������ͨ�����е�һ������0.5����
                if (Random.value > 0.5f)
                {
                    animator.SetTrigger("attack1");     //����attack1
                    Debug.Log("Attack1");
                }
                else
                {
                    animator.SetTrigger("attack2");     //����attack2
                    Debug.Log("Attack2");
                }

                lastAttackTime = coolDown;          //������ȴʱ��
            }
        }
    }

    /// <summary>
    /// ���ù�������sword attack1�������¼�
    /// </summary>
    public void EnableAttackCheckBox1()
    {
        swordAttackCheckBox1.enabled = true;
    }

    /// <summary>
    /// ȡ�����ù�������sword attack1�������¼�
    /// </summary>
    public void UnenableAttackCheckBox1()
    {
        swordAttackCheckBox1.enabled = false;
    }

    /// <summary>
    /// ���ù�������sword attack1�������¼�
    /// </summary>
    public void EnableAttackCheckBox2()
    {
        swordAttackCheckBox2.enabled = true;
    }

    /// <summary>
    /// ȡ�����ù�������sword attack1�������¼�
    /// </summary>
    public void UnenableAttackCheckBox2()
    {
        swordAttackCheckBox2.enabled = false;
    }

    /// <summary>
    /// ���ù�������sword attack1�������¼�
    /// </summary>
    public void EnableAttackCheckBox3()
    {
        swordAttackCheckBox3.enabled = true;
    }

    /// <summary>
    /// ȡ�����ù�������sword attack1�������¼�
    /// </summary>
    public void UnenableAttackCheckBox3()
    {
        swordAttackCheckBox3.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))    //���˹���Player
        {
            if (playerStats.CurrentHealth <= 0) return;

            isHurt = true;
            //������Collider�ĸ���ΪPlayer
            CharacterStats attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            playerStats.TakeDamage(attackerStats, playerStats);   //�����˺������㱻�����ߵ�ǰѪ��

            InjuredFlash();     //������˸
            InjuredBack(collision.gameObject.transform.parent.position);  //���˱�����
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))   //�ȵ�Spike
        {
            if (playerStats.CurrentHealth <= 0) return;

            CharacterStats attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            playerStats.TakeDamage(attackerStats, playerStats);   //�����˺������㱻�����ߵ�ǰѪ��

            InjuredFlash();     //������˸
            InjuredBack(collision.gameObject.transform.parent.position);  //���˱�����
        }
    }

    /// <summary>
    /// ���˱�����Ч��
    /// </summary>
    private void InjuredBack(Vector2 attackerPos)
    {
        float impactForce = 4f;
        rigidbody2d.velocity = ((Vector2)transform.position - attackerPos) * impactForce;
        //rigidbody2d.AddForce(((Vector2)transform.position - attackerPos) * impactForce, ForceMode2D.Impulse);   //��Ӵ�Player��Enemy����
    }

    /// <summary>
    /// ������˸
    /// </summary>
    private void InjuredFlash()
    {
        sprite.material = hurtMaterial; //��������
        sprite.color = Color.red;       //������ɫ
        Invoke("ResetColor", hurtTime);     //�ӳٵ���
    }

    /// <summary>
    /// ������ɫ
    /// </summary>
    private void ResetColor()
    {
        sprite.material = originaMaterial;
        sprite.color = originalColor;
        isHurt = false;
    }
}
