using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum TransitionType
{
    SameScene,      //ͬ����
    DifferentScene  //��ͬ����
}

public class TransitionPoint : MonoBehaviour
{
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public DestinationTag destinationTag;

    private bool canTrans;  //�ܷ���



    private void Update()
    {
        if(canTrans && Input.GetKeyDown(KeyCode.E))
        {
            //����
            SceneController.Instance.TransitionToDestination(this);     //���ͣ���ǰ�������ڵ�Ϊ��㣩
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
