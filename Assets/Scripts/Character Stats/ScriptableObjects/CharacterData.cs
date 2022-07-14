using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Data", menuName = "Character Stats/Basic Data")]    //��������Ҽ��˵���������Data�ļ�
public class CharacterData : ScriptableObject
{
    [Header("States Info")]

    public int maxHealth;
    public int currentHealth;
    public int baseDefence;     //��������
    public int currentDefence;  //��ǰ����

    public float runSpeed;
    public float jumpForce;
    public int maxAirJumpNum;        //������Ծ������n������
    public float rollSpeed;
    public float climbSpeed;

    [Header("Kill Point")]

    public int killPoint;   //����ɱ�ɻ�õ���

    [Header("Level")]

    public int currentLevel;    //��ǰ�ȼ�
    public int maxLevel;        //���ȼ�
    public int baseExp;         //��������ֵ���ﵽ�þ���ֵ������
    public int currentExp;      //��ǰ����ֵ
    public float levelBuff;     //ÿ���ȼ��Ļ�������ֵ�����ӱ��ʵ�����

    public float levelMultiplier    //ÿ���ȼ����ݵ����ӱ���
    {
        get{ return 1 + (currentLevel - 1) * levelBuff; }
    }

    /// <summary>
    /// ���¾���ֵ
    /// </summary>
    /// <param name="point">��õ���</param>
    public void UpdateExp(int point)
    {
        currentExp += point;

        if(currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void LevelUp()
    {
        //���������������ݷ���----------

        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);  //�ȼ�+1 ��0�����ȼ�֮��
        baseExp += (int)(baseExp * levelMultiplier);    //��������ֵ����
        
        maxHealth = (int)(maxHealth * levelMultiplier); //���Ѫ������
        currentHealth = maxHealth;  //��Ѫ

        Debug.Log("Level:" + currentLevel + "Health:" + maxHealth);
    }
}
