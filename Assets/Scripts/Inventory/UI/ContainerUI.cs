using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    public SlotHolder[] slotHolders;    //背包中所有格子

    /// <summary>
    /// 刷新背包UI
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].itemUI.Index = i;    //更新格子物品序号
            slotHolders[i].UpdateItem();        //更新格子物品
        }
    }
}
