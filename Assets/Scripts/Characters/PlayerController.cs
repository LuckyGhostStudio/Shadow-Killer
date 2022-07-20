using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterStats playerStats;     //Player的数据

    [Header("Player组件")]

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer sprite;
    private Transform groundCheck;                      //地面检测Transform

    private PolygonCollider2D swordAttackCheckBox1;          //sword攻击检测collider1
    private PolygonCollider2D swordAttackCheckBox2;          //sword攻击检测collider2
    private PolygonCollider2D swordAttackCheckBox3;          //sword攻击检测collider3

    [SerializeField] private LayerMask groundLayer;     //地面检测图层

    [Header("输入参数")]
    public float horizontal;            //水平输入：A/D <-/->
    public bool jumpPressed;            //跳跃键按下：Space
    public bool rollPressed;            //滚动键按下：C

    [Header("基本参数")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int baseDefence;     //基础防御
    [SerializeField] private int currentDefence;  //当前防御

    [Header("移动参数")]
    [SerializeField] private float speed;               //当前速度
    [SerializeField] private float runSpeed;            //跑步速度
    [SerializeField] private float jumpForce;           //跳跃力
    [SerializeField] private int maxAirJumpNum;         //空中最大可跳跃次数
    [SerializeField] private int currentAirJumpNum;     //当前空中可跳跃次数
    [SerializeField] private float rollSpeed;           //滚动速度
    [SerializeField] private float climbSpeed;          //爬行速度

    [Header("攻击参数")]
    [SerializeField] private float attackRange;         //攻击范围
    [SerializeField] private float coolDown;            //冷却时间
    [SerializeField] private float lastAttackTime;      //上次攻击时间
    [SerializeField] private int maxDamage;             //最大伤害
    [SerializeField] private int minDamage;             //最小伤害
    [SerializeField] private float criticalMultiplier;    //暴击倍率
    [SerializeField] private float criticalChance;      //暴击率
    [SerializeField] private bool isCritical;           //是否暴击

    [SerializeField] private int coinNumber = 0;        //金币数量

    [Header("状态参数")]
    [SerializeField] private bool isAggressive;         //是否有攻击性：是否有武器，用于判断攻击动画是否可启用
    [SerializeField] private bool onGround;             //是否在地面

    [Header("受伤参数")]
    [SerializeField] private float hurtTime = 0.4f;     //受伤持续时间
    private Color originalColor;                        //原始颜色
    [SerializeField] private Material originaMaterial;  //原始材质
    [SerializeField] private Material hurtMaterial;     //受伤材质

    //配合动画切换
    public bool isHurt { get; set; }
    public bool isJump { get; set; }
    public bool isFall { get; set; }
    public bool isIdle { get; set; }
    public bool isRoll { get; set; }
    public bool isDead { get; set; }

    protected override void Awake()     //重写父类Singleton的Awake
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

        InitStats();    //初始化各参数
    }

    //初始化各参数
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
        GameManager.Instance.RegisterPlayer(playerStats);   //将Player的数据传到GameManager
    }

    void Start()
    {
        SaveManager.Instance.LoadPlayerData();  //加载Player的数据
    }

    private void Update()
    {
        isDead = playerStats.CurrentHealth == 0;    //当前血量为0，死亡

        if (isDead) //Player死亡
        {
            Dead(); //死亡
        }
        else if (!SceneUI.Instance.pause) 
        {
            if (lastAttackTime >= 0) lastAttackTime -= Time.deltaTime;

            Attack();   //攻击
            
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
    /// 死亡：Player死亡后的一些操作
    /// </summary>
    private void Dead()
    {
        animator.SetBool("dead", isDead);
        GameManager.Instance.NotifyObservers();     //通知所有Enemy 游戏结束
        GameManager.Instance.GameOver();
    }

    /// <summary>
    /// 地面检测
    /// </summary>
    private void PhysicCheck()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);  //地面检测
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            rigidbody2d.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rigidbody2d.velocity.y);   //移动
            transform.localScale = new Vector3(horizontal > 0 ? 1 : -1, 1, 1);  //左右翻转图片
        }
    }

    /// <summary>
    /// 滚动
    /// </summary>
    private void Roll()
    {
        rollPressed = Input.GetKeyDown(KeyCode.C);

        isRoll = onGround && rollPressed;   

        //需修改
        //if(rollPressed)
        //    rigidbody2d.velocity = new Vector2(transform.localScale.x * rollSpeed * Time.deltaTime, rigidbody2d.velocity.y);    //向面朝的方向移动
        
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        jumpPressed = Input.GetButtonDown("Jump");

        //普通跳
        if (onGround && jumpPressed)     //在地面 按下跳跃键
        {
            speed = runSpeed * 0.9f;    //在空中移动速度减小 
            rigidbody2d.velocity = Vector2.up * jumpForce;
            isJump = true;
        }
        else if(!onGround && rigidbody2d.velocity.y < 0.1f)  //不在地面 下落时
        {
            isJump = false;
            isFall = true;
        }

        //多段跳
        if(onGround && rigidbody2d.velocity.y < 0.1f)   //在地面 没有上升
        {
            speed = runSpeed;
            currentAirJumpNum = maxAirJumpNum;
        }
        else if (!onGround && jumpPressed && currentAirJumpNum > 0)     //不在地面 跳跃 空中可跳跃次数>0
        {
            speed = runSpeed * 0.9f;    //在空中移动速度减小 
            rigidbody2d.velocity = Vector2.up * jumpForce;
            isJump = true;
            currentAirJumpNum--;
        }

        if (isJump && onGround)     //跳跃时 在地面：跳跃最高点时刚好落在地面
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
    /// 攻击
    /// </summary>
    private void Attack()
    {
        if (Input.GetKey(KeyCode.J))
        {
            //判断是否暴击
            isCritical = Random.value < criticalChance;      //[0,1]随机取值，取值小于暴击率的概率刚好为暴击率
            playerStats.isCritical = isCritical;

            if (lastAttackTime < 0)     //冷却结束
            {
                animator.SetBool("critical", isCritical);   //触发暴击，暴击动画

                //随机触发两个普通攻击中的一个：各0.5概率
                if (Random.value > 0.5f)
                {
                    animator.SetTrigger("attack1");     //触发attack1
                    Debug.Log("Attack1");
                }
                else
                {
                    animator.SetTrigger("attack2");     //触发attack2
                    Debug.Log("Attack2");
                }

                lastAttackTime = coolDown;          //重置冷却时间
            }
        }
    }

    /// <summary>
    /// 启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void EnableAttackCheckBox1()
    {
        swordAttackCheckBox1.enabled = true;
    }

    /// <summary>
    /// 取消启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void UnenableAttackCheckBox1()
    {
        swordAttackCheckBox1.enabled = false;
    }

    /// <summary>
    /// 启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void EnableAttackCheckBox2()
    {
        swordAttackCheckBox2.enabled = true;
    }

    /// <summary>
    /// 取消启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void UnenableAttackCheckBox2()
    {
        swordAttackCheckBox2.enabled = false;
    }

    /// <summary>
    /// 启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void EnableAttackCheckBox3()
    {
        swordAttackCheckBox3.enabled = true;
    }

    /// <summary>
    /// 取消启用攻击检测框sword attack1：动画事件
    /// </summary>
    public void UnenableAttackCheckBox3()
    {
        swordAttackCheckBox3.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))    //敌人攻击Player
        {
            if (playerStats.CurrentHealth <= 0) return;

            isHurt = true;
            //攻击的Collider的父级为Player
            CharacterStats attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            playerStats.TakeDamage(attackerStats, playerStats);   //计算伤害，计算被攻击者当前血量

            InjuredFlash();     //受伤闪烁
            InjuredBack(collision.gameObject.transform.parent.position);  //受伤被击退
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))   //踩到Spike
        {
            if (playerStats.CurrentHealth <= 0) return;

            CharacterStats attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            playerStats.TakeDamage(attackerStats, playerStats);   //计算伤害，计算被攻击者当前血量

            InjuredFlash();     //受伤闪烁
            InjuredBack(collision.gameObject.transform.parent.position);  //受伤被击退
        }
    }

    /// <summary>
    /// 受伤被击退效果
    /// </summary>
    private void InjuredBack(Vector2 attackerPos)
    {
        float impactForce = 4f;
        rigidbody2d.velocity = ((Vector2)transform.position - attackerPos) * impactForce;
        //rigidbody2d.AddForce(((Vector2)transform.position - attackerPos) * impactForce, ForceMode2D.Impulse);   //添加从Player到Enemy的力
    }

    /// <summary>
    /// 受伤闪烁
    /// </summary>
    private void InjuredFlash()
    {
        sprite.material = hurtMaterial; //红闪材质
        sprite.color = Color.red;       //红闪颜色
        Invoke("ResetColor", hurtTime);     //延迟调用
    }

    /// <summary>
    /// 重置颜色
    /// </summary>
    private void ResetColor()
    {
        sprite.material = originaMaterial;
        sprite.color = originalColor;
        isHurt = false;
    }
}
