using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy״̬
/// </summary>
public enum EnemyStates
{
    GUARD,      //����
    PATROL,     //Ѳ��
    CHASE,      //׷��
    DEAD        //����
}

[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    public EnemyStates enemyStates;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer sprite;

    private CharacterStats enemyStats;      //Enemy������

    [Header("Ѳ�߲���")]
    [SerializeField] private float patrolRange = 4;     //Ѳ�߷�Χ
    private Vector2 patrolTargetPoint;                  //Ѳ��Ŀ��㣺��Ѳ�߷�Χ��Ҫ�ƶ�����Ŀ���
    private Vector2 guardPos;                           //�����λ�ã�Player���Ѻ�Ҫ���ص�λ��
    private int guardFaceTo;                            //����ʱ�ĳ���Player���Ѻ�Ҫ���صĳ���
    private float lookAtTime = 2;                       //����һ��Ѳ�ߵ����Ҫ�ȴ���ʱ��
    private float remainLookAtTime;                     //��ǰʣ��ȴ�ʱ��

    [Header("��������")]
    //[SerializeField] private int maxHealth;             //���Ѫ��
    //[SerializeField] private int currentHealth;         //��ǰѪ��
    //[SerializeField] private int baseDefence;           //��������
    //[SerializeField] private int currentDefence;        //��ǰ����
    [SerializeField] private bool isGuard;              //�Ǿ���ĵ���
    [SerializeField] private float sightRadius = 5;     //���ӷ�Χ

    [Header("�ƶ�����")]
    [SerializeField] private float speed;               //��ǰ�ٶ�
    [SerializeField] private float runSpeed = 300;            //�ܲ��ٶ�
    //[SerializeField] private float jumpForce;           //��Ծ��
    //[SerializeField] private int maxJumpNum = 2;        //������Ծ����
    //[SerializeField] private int currentJumpNum;        //��ǰ����Ծ����
    //[SerializeField] private float rollSpeed;           //�����ٶ�
    //[SerializeField] private float climbSpeed;          //�����ٶ�

    [Header("��������")]

    //[SerializeField] private float attackRange;         //������Χ
    //[SerializeField] private float coolDown;            //��ȴʱ��
    [SerializeField] private float lastAttackTime;      //�ϴι���ʱ��
    //[SerializeField] private int maxDamage;             //����˺�
    //[SerializeField] private int minDamage;             //��С�˺�
    //[SerializeField] private float criticalMultiplier;  //��������
    //[SerializeField] private float criticalChance;      //������
    //[SerializeField] private bool isCritical;           //�Ƿ񱩻�
    private GameObject attackTarget;                    //����Ŀ��

    [Header("���˲���")]
    [SerializeField] private GameObject damageFloatEffectPrefab;      //�˺�ֵ����Ч��
    [SerializeField] private GameObject impactEffectPrefab;   //��Player������Ч��
    [SerializeField] private float hurtTime = 0.4f;     //���˳���ʱ��
    private Color originalColor;                        //ԭʼ��ɫ
    [SerializeField] private Material originaMaterial;  //ԭʼ����
    [SerializeField] private Material hurtMaterial;     //���˲���

    //��϶����л�
    private bool impactChange;  //������Ч���л�
    private bool isRun;
    private bool isDead;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<CharacterStats>();
        sprite = GetComponent<SpriteRenderer>();

        guardPos = transform.position;  //��ʼ������λ��
        guardFaceTo = (int)transform.localScale.x;  //����ʱ�泯��
        remainLookAtTime = lookAtTime;  //��ʼ���ȴ�ʱ��

        //damageFloatEffectPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/DamageFloatBase.prefab", typeof(GameObject)) as GameObject;
        //impactEffectPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/impact.prefab", typeof(GameObject)) as GameObject;
        originalColor = sprite.color;
        originaMaterial = sprite.material;
        //hurtMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/2D Platformer Tileset/Simple UI Pack/Font/Font Material.mat", typeof(Material)) as Material;

        InitStats();
    }

    //��ʼ��������
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
        if (isGuard)    //�Ǿ���ĵ���
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewPatrolPoint();    //��ʼ��Ѳ�ߵ�
        }

        ///�����л�ʱ�޸Ĵ˲���
        GameManager.Instance.AddObservers(this);    //����ǰ������ӵ��㲥�б�
    }
    //�����л�����ʱ�޸�
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObservers(this);    //����ǰ������ӵ��㲥�б�
    //}

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObservers(this); //����ǰ�����Ƴ��㲥�б�
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
    /// ��Ŀ���ƶ�
    /// </summary>
    /// <param name="target">Ŀ��</param>
    private void MoveToTarget(Vector3 target)
    {
        float horizontal = target.x < transform.position.x ? -1 : 1;    //�ж�Ŀ������߻��ұ�
        transform.localScale = new Vector3(horizontal, 1, 1);
        rigidbody2d.velocity = new Vector2(speed * horizontal * Time.deltaTime, rigidbody2d.velocity.y);    //��Ŀ���ƶ�
    }

    /// <summary>
    /// �л�����
    /// </summary>
    private void SwitchAnimations()
    {
        animator.SetBool("run", isRun);
        animator.SetBool("dead", isDead);
    }

    /// <summary>
    /// �л�Enemy״̬
    /// </summary>
    private void SwicthStates()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;     //����״̬
        }
        else if (FoundPlayer())  //����Player
        {
            enemyStates = EnemyStates.CHASE;    //׷��
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:     //����
                //Idle
                speed = runSpeed;  //�ٶȼ�С
                if (Vector2.Distance(guardPos, transform.position) >= 0.5f)
                {
                    isRun = true;
                    MoveToTarget(guardPos);     //�ص�����λ��
                }
                else
                {
                    isRun = false;
                    transform.localScale = new Vector3(guardFaceTo, 1, 1);
                }

                break;
            case EnemyStates.PATROL:    //Ѳ��
                Patrol();
                break;
            case EnemyStates.CHASE:     //׷��
                Chase();
                break;
            case EnemyStates.DEAD:      //����

                break;
        }
    }

    /// <summary>
    /// Ѳ��״̬
    /// </summary>
    private void Patrol()
    {
        speed = runSpeed;    //��ǰ�ٶ�ΪѲ���ٶ�
        //��Ѳ�߷�Χ������ƶ�

        //Ŀ��Ѳ�ߵ�͵�ǰ�����Ƿ�һ�£��ӽ���
        if (Vector2.Distance(patrolTargetPoint, transform.position) <= 0.5f)
        {
            isRun = false;  //�����л���idle

            if (remainLookAtTime > 0)   //�ȴ�δ����
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else //�ȴ�����
            {
                remainLookAtTime = lookAtTime;  //���õȴ�ʱ��
                GetNewPatrolPoint();    //����µ�Ѳ�ߵ�
            }
        }
        else  //δ����Ѳ�ߵ�
        {
            isRun = true;   //����run
            MoveToTarget(patrolTargetPoint);    //��Ѳ�ߵ��ƶ�
        }
    }

    /// <summary>
    /// ׷��״̬
    /// </summary>
    private void Chase()
    {
        speed = runSpeed * 1.5f;         //�ٶ�����
        isRun = true;   //�����л���run

        if (!FoundPlayer())     //Ŀ�궪ʧ
        {
            isRun = false;      //�����л���idle

            if (remainLookAtTime < 0)   //�ȴ�һ��ʱ��
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)   //�Ǿ���ĵ���
            {
                enemyStates = EnemyStates.GUARD;    //�ص�����״̬
            }
            else
            {
                enemyStates = EnemyStates.PATROL;   //�ص�Ѳ��״̬
            }
        }
        else
        {
            if (!TargetInAttackRange())     //�빥��Ŀ�������ڹ�����Χ
            {
                MoveToTarget(attackTarget.transform.position);     //��Ŀ���ƶ�
            }
            else  //�ڹ�����Χ��
            {
                isRun = false;  //�����л���idle

                if (lastAttackTime < 0)     //��ȴ����
                {
                    lastAttackTime = enemyStats.attackData.coolDown;  //������ȴʱ��

                    //�ж��Ƿ񱩻�
                    enemyStats.isCritical = Random.value < enemyStats.attackData.criticalChance;      //[0,1]���ȡֵ��ȡֵС�ڱ����ʵĸ��ʸպ�Ϊ������
                    //enemyStats.isCritical = isCritical;
                    //����
                    Attack();
                }
            }
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        //if (isCritical)
        //{
        //    //Debug.Log("������");
        //}
        ////Debug.Log("��ͨ����");
    }

    /// <summary>
    /// ����Player
    /// </summary>
    /// <returns>�Ƿ���</returns>
    private bool FoundPlayer()
    {
        var colliders= Physics2D.OverlapCircleAll(transform.position, sightRadius);   //����sightRadius�뾶��Χ������Collier����������

        foreach (var target in colliders)
        {
            if(target.CompareTag("Player"))     //��Player
            {
                attackTarget = target.gameObject;   //����Ŀ����Player
                return true;
            }
        }

        attackTarget = null;    
        return false;
    }

    /// <summary>
    /// ����Ŀ���Ƿ��ڹ�����Χ��
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

    //����µ�Ѳ�ߵ�
    private void GetNewPatrolPoint()
    {
        //��վ׮���Ѳ�߰뾶����������µ�Ѳ�ߵ�
        Vector2 newPoint = new Vector2(guardPos.x + Random.Range(-patrolRange, patrolRange), transform.position.y);

        patrolTargetPoint = newPoint;
    }

    //�ö���ѡ��ʱ����Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);     //���ƿ��ӷ�Χ

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);     //Ѳ�߷�Χ

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStats.attackData.attackRange);    //������Χ
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterStats attackerStats = null;    //������
        int damage = 0; //�ܵ����˺�

        if (collision.CompareTag("PlayerSword"))    //Player Sword����
        {
            //������Collider�ĸ���ΪPlayer
            attackerStats = collision.gameObject.transform.parent.GetComponent<CharacterStats>();
            damage = enemyStats.TakeDamage(attackerStats, enemyStats);   //�����˺������㱻�����ߵ�ǰѪ��      
        }
        else if(collision.CompareTag("Spike"))   //�ȵ�Spike
        {
            attackerStats = collision.gameObject.GetComponent<CharacterStats>();
            damage = enemyStats.TakeDamage(attackerStats, enemyStats);   //�����˺������㱻�����ߵ�ǰѪ��
        }

        if (collision == null || attackerStats == null) return;

        impactEffect(attackerStats, damage);     //��Player����Ч��
        InjuredFlash();     //������˸
        InjuredBack(collision.gameObject.transform.parent.position);  //���˱�����
    }

    /// <summary>
    /// ��Player����Ч�������������˺�ֵ�ͻ���Ч��
    /// </summary>
    private void impactEffect(CharacterStats attacker, int damage)
    {
        //�˺�����Ч��
        GameObject damageFloat = Instantiate(damageFloatEffectPrefab, transform.position, Quaternion.identity);
        TextMesh damageTextMesh = damageFloat.transform.GetChild(0).GetComponent<TextMesh>();
        if (attacker.isCritical)    //����
        {
            damageTextMesh.color = Color.red;   //�˺�ֵΪ��ɫ
        }
        damageTextMesh.text = damage.ToString();
        Destroy(damageFloat, 0.4f);

        Animator impactAnimator = impactEffectPrefab.GetComponent<Animator>();
        GameObject impact = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);   //��ӱ�Player����Ч��
        if (Random.value > 0.5f)
        {
            impactAnimator.SetTrigger("impact2");   //����л�impact����
            //Debug.Log("impact2");
        }
        Destroy(impact, 1.0f / 3);
    }

    /// <summary>
    /// ���˱�����Ч��
    /// </summary>
    private void InjuredBack(Vector2 attackerPos)
    {
        float impactForce = 4.0f;
        
        rigidbody2d.velocity = ((Vector2)transform.position - attackerPos) * impactForce;   
        //rigidbody2d.AddForce(((Vector2)transform.position - attackerPos) * impactForce, ForceMode2D.Impulse);   //��Ӵ�Player��Enemy����
    }

    /// <summary>
    /// ������˸
    /// </summary>
    private void InjuredFlash()
    {
        sprite.material = hurtMaterial; //���˲���
        sprite.color = Color.white;     //������ɫ
        Invoke("ResetColor", hurtTime);     //�ӳٵ���
    }

    /// <summary>
    /// ������ɫ
    /// </summary>
    private void ResetColor()
    {
        sprite.material = originaMaterial;
        sprite.color = originalColor;
    }

    /// <summary>
    /// �����������Ž���ʱ���ã������¼�
    /// </summary>
    public void DestoryEnemy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ��Ϸ����֪ͨ
    /// </summary>
    public void EndNotify()
    {
        enemyStates = EnemyStates.GUARD;    //״̬��Ϊ����״̬ ֹͣ�ƶ�
        attackTarget = null;
        //֮����ܻ�����������
        Debug.Log("EndNotify");
    }
}
