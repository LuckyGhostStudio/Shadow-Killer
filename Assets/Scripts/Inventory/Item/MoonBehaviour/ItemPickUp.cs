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
            //将物品添加到背包
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();  //刷新背包UI


            //GameManager.Instance.playerStats.EquipWeapon(itemData);
            //将地图上的物品删除
            Destroy(gameObject);
        }
    }
}
