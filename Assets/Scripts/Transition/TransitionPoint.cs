using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送类型
/// </summary>
public enum TransitionType
{
    SameScene,      //同场景
    DifferentScene  //不同场景
}

public class TransitionPoint : MonoBehaviour
{
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public DestinationTag destinationTag;

    private bool canTrans;  //能否传送



    private void Update()
    {
        if(canTrans && Input.GetKeyDown(KeyCode.E))
        {
            //传送
            SceneController.Instance.TransitionToDestination(this);     //传送（当前对象所在点为起点）
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrans = true;
            SceneUI.Instance.SetTipsDialog(canTrans);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrans = false;
            SceneUI.Instance.SetTipsDialog(canTrans);
        }
    }
}
