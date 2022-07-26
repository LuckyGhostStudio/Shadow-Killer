using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;      //��ƷͼƬ
    public Text amount;     //��Ʒ����

    public InventoryData Bag { get; set; }  //�������
    public int Index { get; set; } = -1;    //��Ʒ��ţ��ڱ����е���ţ��ڿ���б��е�������

    /// <summary>
    /// ������ƷUI
    /// </summary>
    /// <param name="itemData">��Ʒ����</param>
    /// <param name="itemAmount">����</param>
    public void SetupItemUI(ItemData itemData, int itemAmount)
    {
        if (itemData != null)
        {
            icon.sprite = itemData.itemIcon;
            amount.text = itemAmount.ToString();

            icon.gameObject.SetActive(true);    //����icon���ڶ���
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }
}
