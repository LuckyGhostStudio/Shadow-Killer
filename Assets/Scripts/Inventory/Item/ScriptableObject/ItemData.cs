using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Usable,     //可使用的
    Weapon      //武器
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;   //类型
    public string itemName;     //名字
    public Sprite itemIcon;     //图标
    public int itemAmount;      //数量

    [TextArea]
    public string description;  //物品描述
    public bool stackable;      //是否是可堆叠的
}
