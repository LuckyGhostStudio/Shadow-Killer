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
        //��¼ԭʼ������Ϣ
        InventoryManager.Instance.currentDragData = new DragData();
        InventoryManager.Instance.currentDragData.originalHolder = GetComponentInParent<SlotHolder>();  //��ǰ����
        InventoryManager.Instance.currentDragData.originalParent = (RectTransform)transform.parent;     //��ǰ���ӵ�Transform

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);  //���õ�ǰ���󸸼�ΪdragCanvas�����ֵ�ǰ����������UI�Ϸ���ʾ
    }

    public void OnDrag(PointerEventData eventData)
    {
        //��Ʒ��������ƶ�
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ ��������
    }
}
