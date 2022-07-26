using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    //TODO:������ģ�����ڱ�������
    [Header("Inventory Data")]
    public InventoryData inventoryData;

    [Header("Containers")]
    public ContainerUI inventoryUI; //���UI

    private void Start()
    {
        inventoryUI.RefreshUI();
    }
}
