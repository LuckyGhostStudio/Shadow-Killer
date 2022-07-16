using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            animator.SetBool("pickUp", true);   //Coin���Ե��Ķ���
        }
    }

    /// <summary>
    /// pickUp�������Ž���ʱ���ã������¼�
    /// </summary>
    public void DestoryCoin()      
    {
        //�������ټ������������������ظ�����
        Destroy(gameObject);
        GameManager.Instance.playerStats.characterData.coinNumber++;    //Coin����+1
    }
}
