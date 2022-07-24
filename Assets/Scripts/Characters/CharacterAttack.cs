using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private CharacterStats characterStats;                      //character的数据

    private Animator animator;

    private PolygonCollider2D swordAttackCheckBox1;             //sword攻击检测collider1
    private PolygonCollider2D swordAttackCheckBox2;             //sword攻击检测collider2
    private PolygonCollider2D swordAttackCheckBox3;             //sword攻击检测collider3

    [SerializeField] private float coolDown;            //冷却时间
    [SerializeField] private float lastAttackTime;      //上次攻击时间
    [SerializeField] private bool isCritical;           //是否暴击

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();

        animator = GetComponent<Animator>();

        swordAttackCheckBox1 = transform.GetChild(1).GetComponent<PolygonCollider2D>();
        swordAttackCheckBox2 = transform.GetChild(2).GetComponent<PolygonCollider2D>();
        swordAttackCheckBox3 = transform.GetChild(3).GetComponent<PolygonCollider2D>();

        coolDown = characterStats.attackData.coolDown;
        lastAttackTime = -1;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (lastAttackTime >= 0) lastAttackTime -= Time.deltaTime;

        if (Input.GetKey(KeyCode.J))
        {
            Attack();
        }
           
    }

    private void Attack()
    {
        if (lastAttackTime < 0)     //冷却结束
        {
            /*
            //判断是否暴击
            isCritical = Random.value < criticalChance;      //[0,1]随机取值，取值小于暴击率的概率刚好为暴击率
            playerStats.isCritical = isCritical;

            if (isCritical)
            {
                animator.SetBool("critical", isCritical);   //触发暴击，暴击动画
                Debug.Log("暴击！");
            }
            */
            animator.SetTrigger("attack1");     //触发attack1
            Debug.Log("Attack1");

            lastAttackTime = coolDown;          //重置冷却时间
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
}
