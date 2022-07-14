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
        animator.SetBool("idle", PlayerController.Instance.isIdle);                 //æ≤÷π
        animator.SetFloat("run", Mathf.Abs(PlayerController.Instance.horizontal));  //≈‹
        animator.SetBool("jump", PlayerController.Instance.isJump);                 //Ã¯
        animator.SetBool("fall", PlayerController.Instance.isFall);                 //œ¬¬‰
        animator.SetBool("roll", PlayerController.Instance.isRoll);                 //πˆ∂Ø
        animator.SetBool("hurt", PlayerController.Instance.isHurt);                 // ‹…À
    }
}
