using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 正在拖拽的物品数据
/// </summary>
public class DragData
{
    public SlotHolder originalHolder;       //原来的格子
    public RectTransform originalParent;    //原来的父级
}

public class InventoryManager : Singleton<InventoryManager>
{
    //TODO:最后添加模板用于保存数据
    [Header("Inventory Data")]
    public InventoryData inventoryData;     //库存数据
    public InventoryData actionData;        //活动栏数据
    public InventoryData equipmentData;     //装备栏数据

    [Header("Containers")]
    public ContainerUI inventoryUI;     //库存UI
    public ContainerUI actionUI;        //活动栏UI
    public ContainerUI equipmentUI;     //装备栏UI

    [Header("Drag Canvas")]
    public Canvas dragCanvas;   //拖拽时物品UI的Canvas

    public DragData currentDragData;    //当前拖拽的数据

    private void Start()
    {
        //刷新背包UI
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
}
