using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        animator.SetBool("idle", PlayerController.Instance.isIdle);                 //��ֹ
        animator.SetFloat("run", Mathf.Abs(PlayerController.Instance.horizontal));  //��
        animator.SetBool("jump", PlayerController.Instance.isJump);                 //��
        animator.SetBool("fall", PlayerController.Instance.isFall);                 //����
        animator.SetBool("roll", PlayerController.Instance.isRoll);                 //����
        animator.SetBool("hurt", PlayerController.Instance.isHurt);                 //����
    }
}
