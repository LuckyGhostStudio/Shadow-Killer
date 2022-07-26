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
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();  //ˢ�±���UI


            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //����ͼ�ϵ���Ʒɾ��
            Destroy(gameObject);
        }
    }
}
