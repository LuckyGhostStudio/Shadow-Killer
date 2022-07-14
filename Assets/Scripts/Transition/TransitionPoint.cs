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
    public GameObject tipsDialogPanel;     //��ʾ�Ի���

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
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrans = true;
            tipsDialogPanel.gameObject.SetActive(canTrans);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrans = false;
            tipsDialogPanel.gameObject.SetActive(canTrans);
        }
    }
}
