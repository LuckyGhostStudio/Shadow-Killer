using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ק����Ʒ����
/// </summary>
public class DragData
{
    public SlotHolder originalHolder;       //ԭ���ĸ���
    public RectTransform originalParent;    //ԭ���ĸ���
}

public class InventoryManager : Singleton<InventoryManager>
{
    //TODO:������ģ�����ڱ�������
    [Header("Inventory Data")]
    public InventoryData inventoryData;     //�������
    public InventoryData actionData;        //�������
    public InventoryData equipmentData;     //װ��������

    [Header("Containers")]
    public ContainerUI inventoryUI;     //���UI
    public ContainerUI actionUI;        //���UI
    public ContainerUI equipmentUI;     //װ����UI

    [Header("Drag Canvas")]
    public Canvas dragCanvas;   //��קʱ��ƷUI��Canvas

    public DragData currentDragData;    //��ǰ��ק������

    private void Start()
    {
        //ˢ�±���UI
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
}
