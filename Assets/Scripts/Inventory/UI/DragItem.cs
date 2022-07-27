using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据信息
        InventoryManager.Instance.currentDragData = new DragData();
        InventoryManager.Instance.currentDragData.originalHolder = GetComponentInParent<SlotHolder>();  //当前格子
        InventoryManager.Instance.currentDragData.originalParent = (RectTransform)transform.parent;     //当前格子的Transform

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);  //设置当前对象父级为dragCanvas：保持当前对象在所有UI上方显示
    }

    public void OnDrag(PointerEventData eventData)
    {
        //物品跟随鼠标移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
    }
}
