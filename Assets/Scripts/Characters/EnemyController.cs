using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy状态
/// </summary>
public enum EnemyStates
{
    GUARD,      //警戒
    PATROL,     //巡逻
    CHASE,      //追击
    DEAD        //死亡
}

[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    public EnemyStates enemyStates;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer sprite;

    private CharacterStats enemyStats;      //Enemy的数据

    [Header("巡逻参数")]
    [SerializeField] private float patrolRange = 4;     //巡逻范围
    private Vector2 patrolTargetPoint;                  //巡逻目标点：在巡逻范围内要移动到的目标点
    private Vector2 guardPos;                           //警戒的位置：Player拉脱后要返回的位置
    private int guardFaceTo;                            //警戒时的朝向：Player拉脱后要返回的朝向
    private float lookAtTime = 2;                       //到达一个巡逻点后需要等待的时间
    private float remainLookAtTime;                     //当前剩余等待时间

    [Header("基本参数")]
    //[SerializeField] private int maxHealth;             //最大血量
    //[SerializeField] private int currentHealth;         //当前血量
    //[SerializeField] private int baseDefence;           //基础防御
    //[SerializeField] private int currentDefence;        //当前防御
    [SerializeField] private bool isGuard;              //是警戒的敌人
    [SerializeField] private float sightRadius = 5;     //可视范围

    [Header("移动参数")]
    [SerializeField] private float speed;               //当前速度
    [SerializeField] private float runSpeed = 300;            //跑步速度
    //[SerializeField] private float jumpForce;           //跳跃力
    //[SerializeField] private int maxJumpNum = 2;        //最大可跳跃次数
    //[SerializeField] private int currentJumpNum;        //当前可跳跃次数
    //[SerializeField] private float rollSpeed;           //滚动速度
    //[SerializeField] private float climbSpeed;          //爬行速度

    [Header("攻击参数")]

    //[SerializeField] private float attackRange;         //攻击范围
    //[SerializeField] private float coolDown;            //冷却时间
    [SerializeField] private float lastAttackTime;      //上次攻击时间
    //[SerializeField] private int maxDamage;             //最大伤害
    //[SerializeField] private int minDamage;             //最小伤害
    //[SerializeField] private float criticalMultiplier;  //暴击倍率
    //[SerializeField] private float criticalChance;      //暴击率
    //[SerializeField] private bool isCritical;           //是否暴击
    private GameObject attackTarget;                    //攻击目标

    [Header("受伤参数")]
    [SerializeField] private GameObject damageFloatEffectPrefab;      //伤害值浮动效果
    [SerializeField] private GameObject impactEffectPrefab;   //被Player攻击的效果
    [SerializeField] private float hurtTime = 0.4f;     //受伤持续时间
    private Color originalColor;                        //原始颜色
    [SerializeField] private Material originaMaterial;  //原始材质
    [SerializeField] private Material hurtMaterial;     //受伤材质

    //配合动画切换
    private bool impactChange;  //被攻击效果切换
    private bool isRun;
    private bool isDead;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<CharacterStats>();
        sprite = GetComponent<SpriteRenderer>();

        guardPos = transform.position;  //初始化警戒位置
        guardFaceTo = (int)transform.localScale.x;  //警戒时面朝向
        remainLookAtTime = lookAtTime;  //初始化等待时间

        //damageFloatEffectPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/DamageFloatBase.prefab", typeof(GameObject)) as GameObject;
        //impactEffectPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/impact.prefab", typeof(GameObject)) as GameObject;
        originalColor = sprite.color;
        originaMaterial = sprite.material;
        //hurtMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/2D Platformer Tileset/Simple UI Pack/Font/Font Material.mat", typeof(Material)) as Material;

        InitStats();
    }

    //初始化各参数
    void InitStats()
    {
        //maxHealth = enemyStats.MaxHealth;
        //currentHealth = enemyStats.CurrentHealth;
        //baseDefence = enemyStats.BaseDefence;
        //currentDefence = enemyStats.CurrentDefence;

        //runSpeed = enemyStats.characterData.runSpeed;
        speed = runSpeed;
        //jumpForce = enemyStats.characterData.jumpForce;
        //maxJumpNum = enemyStats.characterData.maxJumpNum;
        //rollSpeed = enemyStats.characterData.rollSpeed;
        //climbSpeed = enemyStats.characterData.climbSpeed;

        //attackRange = enemyStats.attackData.attackRange;
        //coolDown = enemyStats.attackData.coolDown;
        //maxDamage = enemyStats.attackData.maxDamage;
        //minDamage = enemyStats.attackData.minDamage;
        //criticalMultiplier = enemyStats.attackData.criticalMultiplier;
        //criticalChance = enemyStats.attackData.criticalChance;
        lastAttackTime = -1;
    }

    void Start()
    {
        if (isGuard)    //是警戒的敌人
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewPatrolPoint();    //初始化巡逻点
        }

        ///场景切换时修改此部分
        GameManager.Instance.AddObservers(this);    //将当前对象添加到广播列表
    }
    //后面切换场景时修改
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObservers(this);    //将当前对象添加到广播列表
    //}

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObservers(this); //将当前对象移除广播列表
    }

    void Update()
    {
        if (enemyStats.CurrentHealth == 0)
        {
            isDead = true;
        }

        if (!SceneUI.Instance.pause)
        {
            SwicthStates();
            if (lastAttackTime >= 0) lastAttackTime -= Time.deltaTime;
        }
        SwitchAnimations();
    }

    /// <summary>
    /// 向目标移动
    /// </summary>
    /// <param name="target">目标</param>
    private void MoveToTarget(Vector3 target)
    {
        float horizontal = target.x < transform.position.x ? -1 : 1;    //判断目标在左边或右边
        transform.localScale = new Vector3(horizontal, 1, 1);
        rigidbody2d.velocity = new Vector2(speed * horizontal * Time.deltaTime, rigidbody2d.velocity.y);    //向目标移动
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    private void SwitchAnimations()
    {
        animator.SetBool("run", isRun);
        animator.SetBool("dead", isDead);
    }

    /// <summary>
    /// 切换Enemy状态
    /// </summary>
    private void SwicthStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;     //死亡状态
        }
        else if (FoundPlayer())  //发现Player
        {
            enemyStates = EnemyStates.CHASE;    //追击
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:     //警戒
                //Idle
                speed = runSpeed;  //速度减小
                if (Vector2.Distance(guardPos, transform.position) >= 0.5f)
                {
                    isRun = true;
                    MoveToTarget(guardPos);     //回到警戒位置
                }
                else
                {
                    isRun = false;
                    transform.localScale = new Vector3(guardFaceTo, 1, 1);
                }

                break;
            case EnemyStates.PATROL:    //巡逻
                Patrol();
                break;
            case EnemyStates.CHASE:     //追击
                Chase();
                break;
            case EnemyStates.DEAD:      //死亡

                break;
        }
    }

    /// <summary>
    /// 巡逻状态
    /// </summary>
    private void Patrol()
    {
        speed = runSpeed;    //当前速度为巡逻速度
        //在巡逻范围内随机移动

        //目标巡逻点和当前坐标是否一致（接近）
        if (Vector2.Distance(patrolTargetPoint, transform.position) <= 0.5f)
        {
            isRun = false;  //动画切换到idle

            if (remainLookAtTime > 0)   //等待未结束
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else //等待结束
            {
                remainLookAtTime = lookAtTime;  //重置等待时间
                GetNewPatrolPoint();    //获得新的巡逻点
            }
        }
        else  //未到达巡逻点
        {
            isRun = true;   //动画run
            MoveToTarget(patrolTargetPoint);    //向巡逻点移动
        }
    }

    /// <summary>
    /// 追击状态
    /// </summary>
    private void Chase()
    {
        speed = runSpeed * 1.5f;         //速度提升
        isRun = true;   //动画切换到run

        if (!FoundPlayer())     //目标丢失
        {
            isRun = false;      //动画切换到idle

            if (remainLookAtTime < 0)   //等待一段时间
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)   //是警戒的敌人
            {
                enemyStates = EnemyStates.GUARD;    //回到警戒状态
            }
            else
            {
                enemyStates = EnemyStates.PATROL;   //回到巡逻状态
            }
        }
        else
        {
            if (!TargetInAttackRange())     //与攻击目标距离大于攻击范围
            {
                MoveToTarget(attackTarget.transform.position);     //向目标移动
            }
            else  //在攻击范围内
            {
                isRun = false;  //动画切换到idle

                if (lastAttackTime < 0)     //冷却结束
                {
                    lastAttackTime = enemyStats.attackData.coolDown;  //重置冷却时间

                    //判断是否暴击
                    enemyStats.isCritical = Random.value < enemyStats.attackData.criticalChance;      //[0,1]随机取值，取值小于暴击率的概率刚好为暴击率
                    //enemyStats.isCritical = isCritical;
                    //攻击
                    Attack();
                }
            }
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    private void Attack()
    {
        //if (isCritical)
        //{
        //    //Debug.Log("暴击！");
        //}
        ////Debug.Log("普通攻击");
    }

    /// <summary>
    /// 发现Player
    /// </summary>
    /// <returns>是否发现</returns>
    private bool FoundPlayer()
    {
        var colliders= Physics2D.OverlapCircleAll(transform.position, sightRadius);   //查找sightRadius半径范围内所有Collier，返回数组

        foreach (var target in colliders)
        {
            if(target.CompareTag("Player"))     //是Player
            {
                attackTarget = target.gameObject;   //攻击目标是Player
                return true;
            }
        }

        attackTarget = null;    
        return false;
    }

    /// <summary>
    /// 攻击目标是否在攻击范围内
    /// </summary>
    /// <returns></returns>
    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector2.Distance(attackTarget.transform.position, transform.position) <= enemyStats.attackData.attackRange;
        }
        else
        {
            return false;
        }
    }

    //获得新的巡逻点
    private void GetNewPatrolPoint()
    {
        //在站桩点的巡逻半径内随机生成新的巡逻点
        Vector2 newPoint = new Vector2(guardPos.x + Random.Range(-patrolRange, patrolRange), transform.position.y);

        patrolTargetPoint = newPoint;
    }

    //该对象选中时绘制Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);     //绘制可视范围

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);     //巡逻范围

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStats.attackData.attackRange);    //攻击范围
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterStats attackerStats = null;    //攻击者
        int damage = 0; //受到的伤害

        if (collision.CompareTag("PlayerSword"))    //Player Sword攻击
        {
            //攻击的Collider的父级为Player
            attackerStats = collision.gameObject.transform.parent.GetComponent<CharacterStats>();
            damage = enemyStats.TakeDamage(attackerStats, enemyStats);   //计算伤害，计算被攻击者当前血量      
        }
        else if(collision.CompareTag("Spike"))   //踩到Spike
        {
            attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            damage = enemyStats.TakeDamage(attackerStats, enemyStats);   //计算伤害，计算被攻击者当前血量
        }

        if (collision == null || attackerStats == null) return;

        impactEffect(attackerStats, damage);     //被Player击打效果
        InjuredFlash();     //受伤闪烁
        InjuredBack(collision.gameObject.transform.parent.position);  //受伤被击退
    }

    /// <summary>
    /// 被Player击打效果：产生浮动伤害值和击打效果
    /// </summary>
    private void impactEffect(CharacterStats attacker, int damage)
    {
        //伤害浮动效果
        GameObject damageFloat = Instantiate(damageFloatEffectPrefab, transform.position, Quaternion.identity);
        TextMesh damageTextMesh = damageFloat.transform.GetChild(0).GetComponent<TextMesh>();
        if (attacker.isCritical)    //暴击
        {
            damageTextMesh.color = Color.red;   //伤害值为红色
        }
        damageTextMesh.text = damage.ToString();
        Destroy(damageFloat, 0.4f);

        Animator impactAnimator = impactEffectPrefab.GetComponent<Animator>();
        GameObject impact = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);   //添加被Player击打效果
        if (Random.value > 0.5f)
        {
            impactAnimator.SetTrigger("impact2");   //随机切换impact动画
            //Debug.Log("impact2");
        }
        Destroy(impact, 1.0f / 3);
    }

    /// <summary>
    /// 受伤被击退效果
    /// </summary>
    private void InjuredBack(Vector2 attackerPos)
    {
        float impactForce = 4.0f;
        
        rigidbody2d.velocity = ((Vector2)transform.position - attackerPos) * impactForce;   
        //rigidbody2d.AddForce(((Vector2)transform.position - attackerPos) * impactForce, ForceMode2D.Impulse);   //添加从Player到Enemy的力
    }

    /// <summary>
    /// 受伤闪烁
    /// </summary>
    private void InjuredFlash()
    {
        sprite.material = hurtMaterial; //受伤材质
        sprite.color = Color.white;     //受伤颜色
        Invoke("ResetColor", hurtTime);     //延迟调用
    }

    /// <summary>
    /// 重置颜色
    /// </summary>
    private void ResetColor()
    {
        sprite.material = originaMaterial;
        sprite.color = originalColor;
    }

    /// <summary>
    /// 死亡动画播放结束时调用：动画事件
    /// </summary>
    public void DestoryEnemy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 游戏结束通知
    /// </summary>
    public void EndNotify()
    {
        enemyStates = EnemyStates.GUARD;    //状态变为警戒状态 停止移动
        attackTarget = null;
        //之后可能还有其他操作
        Debug.Log("EndNotify");
    }
}
