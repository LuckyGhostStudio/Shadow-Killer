using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //����Ʒ��ӵ�����

            //����ͼ�ϵ���Ʒɾ��
            Destroy(gameObject);
        }
    }
}
