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
            animator.SetBool("pickUp", true);   //Coin被吃掉的动画
        }
    }

    /// <summary>
    /// pickUp动画播放结束时调用：动画事件
    /// </summary>
    public void DestoryCoin()      
    {
        //先销毁再计算数量，避免数量重复计算
        Destroy(gameObject);
        GameManager.Instance.playerStats.characterData.coinNumber++;    //Coin个数+1
    }
}
