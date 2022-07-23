using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Usable,     //��ʹ�õ�
    Weapon      //����
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;

    [TextArea]
    public string description;
    public bool stackable;  //�ɶѵ���
}
