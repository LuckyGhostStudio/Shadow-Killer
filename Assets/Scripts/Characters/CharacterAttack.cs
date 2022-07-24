using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private CharacterStats characterStats;                      //character������

    private Animator animator;

    private PolygonCollider2D swordAttackCheckBox1;             //sword�������collider1
    private PolygonCollider2D swordAttackCheckBox2;             //sword�������collider2
    private PolygonCollider2D swordAttackCheckBox3;             //sword�������collider3

    [SerializeField] private float coolDown;            //��ȴʱ��
    [SerializeField] private float lastAttackTime;      //�ϴι���ʱ��
    [SerializeField] private bool isCritical;           //�Ƿ񱩻�

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
        if (lastAttackTime < 0)     //��ȴ����
        {
            /*
            //�ж��Ƿ񱩻�
            isCritical = Random.value < criticalChance;      //[0,1]���ȡֵ��ȡֵС�ڱ����ʵĸ��ʸպ�Ϊ������
            playerStats.isCritical = isCritical;

            if (isCritical)
            {
                animator.SetBool("critical", isCritical);   //������������������
                Debug.Log("������");
            }
            */
            animator.SetTrigger("attack1");     //����attack1
            Debug.Log("Attack1");

            lastAttackTime = coolDown;          //������ȴʱ��
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
}
